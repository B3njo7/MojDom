using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojDom.Core.Entities;
using MojDom.Core.Enums;
using MojDom.Core.Interfaces;
using System.Security.Claims;

namespace MojDom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropertyManagersController : ControllerBase
    {
        private readonly IPropertyManagerService _managerService;

        public PropertyManagersController(IPropertyManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? zone)
        {
            var managers = await _managerService.GetAllAsync(search, zone);
            return Ok(managers.Select(m => new
            {
                m.Id,
                m.Bio,
                m.Rating,
                m.CompletedInspections,
                m.CoverageZone,
                m.IsAvailable,
                User = new { m.User.FirstName, m.User.LastName, m.User.Email, City = m.User.City?.Name }
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var manager = await _managerService.GetByIdAsync(id);
            if (manager == null)
                return NotFound(new { message = "Manager nije pronađen." });

            return Ok(new
            {
                manager.Id,
                manager.Bio,
                manager.Rating,
                manager.CompletedInspections,
                manager.CoverageZone,
                manager.IsAvailable,
                User = new { manager.User.FirstName, manager.User.LastName, manager.User.Email, City = manager.User.City?.Name },
                ActiveAgreements = manager.Agreements.Count(a => a.Status == AgreementStatus.Active)
            });
        }

        [HttpGet("my-profile")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var manager = await _managerService.GetByUserIdAsync(userId);
            if (manager == null)
                return NotFound(new { message = "Profil nije pronađen." });

            return Ok(new
            {
                manager.Id,
                manager.Bio,
                manager.Rating,
                manager.CompletedInspections,
                manager.CoverageZone,
                manager.IsAvailable,
                User = new { manager.User.FirstName, manager.User.LastName, manager.User.Email, City = manager.User.City?.Name }
            });
        }

        [HttpPut("my-profile")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateManagerProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var manager = await _managerService.GetByUserIdAsync(userId);
            if (manager == null)
                return NotFound(new { message = "Profil nije pronađen." });

            manager.Bio = request.Bio;
            manager.CoverageZone = request.CoverageZone;
            manager.IsAvailable = request.IsAvailable;

            await _managerService.UpdateAsync(manager);
            return Ok(new { message = "Profil uspješno ažuriran." });
        }

        [HttpPost("invite")]
        [Authorize(Roles = "PropertyOwner")]
        public async Task<IActionResult> SendInvitation([FromBody] SendInvitationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var invitation = new ManagerInvitation
            {
                OwnerId = userId,
                PropertyManagerId = request.PropertyManagerId,
                PropertyId = request.PropertyId,
                Message = request.Message,
                Status = InvitationStatus.Pending
            };

            await _managerService.SendInvitationAsync(invitation);
            return Ok(new { message = "Pozivnica uspješno poslana." });
        }

        [HttpGet("invitations")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> GetMyInvitations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var manager = await _managerService.GetByUserIdAsync(userId);
            if (manager == null)
                return NotFound();

            var invitations = await _managerService.GetInvitationsForManagerAsync(manager.Id);
            return Ok(invitations.Select(i => new
            {
                i.Id,
                i.Status,
                i.Message,
                i.SentAt,
                Property = new { i.Property.Id, i.Property.Name, i.Property.Address, City = i.Property.City?.Name },
                Owner = new { i.Owner.FirstName, i.Owner.LastName, i.Owner.Email }
            }));
        }

        [HttpGet("my-invitations")]
        [Authorize(Roles = "PropertyOwner")]
        public async Task<IActionResult> GetSentInvitations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var invitations = await _managerService.GetSentInvitationsAsync(userId);
            return Ok(invitations.Select(i => new
            {
                i.Id,
                i.Status,
                i.Message,
                i.SentAt,
                i.RespondedAt,
                Property = new { i.Property.Id, i.Property.Name },
                Manager = new { i.PropertyManager.User.FirstName, i.PropertyManager.User.LastName }
            }));
        }

        [HttpPut("invitations/{id}/respond")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> RespondToInvitation(int id, [FromBody] RespondInvitationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var manager = await _managerService.GetByUserIdAsync(userId);
            if (manager == null)
                return NotFound();

            var result = await _managerService.RespondToInvitationAsync(id, manager.Id, request.Accept, request.MonthlyFee, request.Terms);

            if (result == null && request.Accept)
                return BadRequest(new { message = "Pozivnica nije pronađena ili je već obrađena." });

            return Ok(new { message = request.Accept ? "Pozivnica prihvaćena, ugovor kreiran." : "Pozivnica odbijena." });
        }
    }

    public class UpdateManagerProfileRequest
    {
        public string? Bio { get; set; }
        public string CoverageZone { get; set; } = null!;
        public bool IsAvailable { get; set; }
    }

    public class SendInvitationRequest
    {
        public int PropertyManagerId { get; set; }
        public int PropertyId { get; set; }
        public string? Message { get; set; }
    }

    public class RespondInvitationRequest
    {
        public bool Accept { get; set; }
        public decimal? MonthlyFee { get; set; }
        public string? Terms { get; set; }
    }
}