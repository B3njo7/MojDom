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
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? cityId, [FromQuery] int? type)
        {
            var properties = await _propertyService.GetAllAsync(search, cityId, type);
            return Ok(properties.Select(p => new
            {
                p.Id,
                p.Name,
                p.Address,
                p.Latitude,
                p.Longitude,
                p.SizeM2,
                p.Status,
                p.Type,
                City = p.City?.Name,
                CoverImage = p.Images.FirstOrDefault(i => i.IsCover)?.ImageUrl ?? p.Images.FirstOrDefault()?.ImageUrl
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound(new { message = "Nekretnina nije pronađena." });

            return Ok(new
            {
                property.Id,
                property.Name,
                property.Address,
                property.Latitude,
                property.Longitude,
                property.SizeM2,
                property.Status,
                property.Type,
                property.Description,
                property.HasSecuritySystem,
                City = new { property.City?.Id, property.City?.Name },
                Owner = new { property.Owner?.Id, property.Owner?.FirstName, property.Owner?.LastName, property.Owner?.Email },
                Images = property.Images.Select(i => new { i.Id, i.ImageUrl, i.IsCover, i.Description }),
                Agreements = property.Agreements.Select(a => new
                {
                    a.Id,
                    a.Status,
                    a.MonthlyFee,
                    a.StartDate,
                    Manager = new { a.PropertyManager?.User?.FirstName, a.PropertyManager?.User?.LastName }
                })
            });
        }

        [HttpGet("my")]
        [Authorize(Roles = "PropertyOwner")]
        public async Task<IActionResult> GetMyProperties()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var properties = await _propertyService.GetByOwnerAsync(userId);
            return Ok(properties.Select(p => new
            {
                p.Id,
                p.Name,
                p.Address,
                p.Status,
                p.Type,
                p.SizeM2,
                City = p.City?.Name,
                CoverImage = p.Images.FirstOrDefault(i => i.IsCover)?.ImageUrl ?? p.Images.FirstOrDefault()?.ImageUrl
            }));
        }

        [HttpPost]
        [Authorize(Roles = "PropertyOwner,Admin")]
        public async Task<IActionResult> Create([FromBody] CreatePropertyRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var property = new Property
            {
                Name = request.Name,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Type = (PropertyType)request.Type,
                Description = request.Description,
                SizeM2 = request.SizeM2,
                CityId = request.CityId,
                OwnerId = userId
            };

            var created = await _propertyService.CreateAsync(property);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { created.Id, created.Name });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "PropertyOwner,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyRequest request)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound(new { message = "Nekretnina nije pronađena." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdmin = User.IsInRole("Admin");

            if (property.OwnerId != userId && !isAdmin)
                return Forbid();

            property.Name = request.Name;
            property.Address = request.Address;
            property.Latitude = request.Latitude;
            property.Longitude = request.Longitude;
            property.Type = (PropertyType)request.Type;
            property.Description = request.Description;
            property.SizeM2 = request.SizeM2;
            property.CityId = request.CityId;
            property.Status = (PropertyStatus)request.Status;

            await _propertyService.UpdateAsync(property);
            return Ok(new { message = "Nekretnina uspješno ažurirana." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "PropertyOwner,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound(new { message = "Nekretnina nije pronađena." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdmin = User.IsInRole("Admin");

            if (property.OwnerId != userId && !isAdmin)
                return Forbid();

            await _propertyService.DeleteAsync(id);
            return Ok(new { message = "Nekretnina uspješno obrisana." });
        }
    }

    public class CreatePropertyRequest
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Type { get; set; }
        public string? Description { get; set; }
        public int SizeM2 { get; set; }
        public int CityId { get; set; }
    }

    public class UpdatePropertyRequest : CreatePropertyRequest
    {
        public int Status { get; set; }
    }
}