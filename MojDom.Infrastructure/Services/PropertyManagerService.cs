using Microsoft.EntityFrameworkCore;
using MojDom.Core.Entities;
using MojDom.Core.Enums;
using MojDom.Core.Interfaces;
using MojDom.Infrastructure.Data;

namespace MojDom.Infrastructure.Services
{
    public class PropertyManagerService : IPropertyManagerService
    {
        private readonly ApplicationDbContext _context;

        public PropertyManagerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PropertyManager>> GetAllAsync(string? search, string? zone)
        {
            var query = _context.PropertyManagers
                .Include(m => m.User)
                    .ThenInclude(u => u.City)
                .Where(m => m.IsAvailable)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(m => m.User.FirstName.Contains(search) || m.User.LastName.Contains(search));
            if (!string.IsNullOrEmpty(zone))
                query = query.Where(m => m.CoverageZone.Contains(zone));

            return await query.ToListAsync();
        }

        public async Task<PropertyManager?> GetByIdAsync(int id)
            => await _context.PropertyManagers
                .Include(m => m.User)
                    .ThenInclude(u => u.City)
                .Include(m => m.Agreements)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<PropertyManager?> GetByUserIdAsync(string userId)
            => await _context.PropertyManagers
                .Include(m => m.User)
                    .ThenInclude(u => u.City)
                .FirstOrDefaultAsync(m => m.UserId == userId);

        public async Task<PropertyManager> UpdateAsync(PropertyManager manager)
        {
            _context.PropertyManagers.Update(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task<ManagerInvitation> SendInvitationAsync(ManagerInvitation invitation)
        {
            await _context.ManagerInvitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
            return invitation;
        }

        public async Task<IEnumerable<ManagerInvitation>> GetInvitationsForManagerAsync(int managerId)
            => await _context.ManagerInvitations
                .Include(i => i.Property)
                    .ThenInclude(p => p.City)
                .Include(i => i.Owner)
                .Where(i => i.PropertyManagerId == managerId)
                .ToListAsync();

        public async Task<IEnumerable<ManagerInvitation>> GetSentInvitationsAsync(string ownerId)
            => await _context.ManagerInvitations
                .Include(i => i.Property)
                .Include(i => i.PropertyManager)
                    .ThenInclude(m => m.User)
                .Where(i => i.OwnerId == ownerId)
                .ToListAsync();

        public async Task<ManagementAgreement?> RespondToInvitationAsync(int invitationId, int managerId, bool accept, decimal? monthlyFee, string? terms)
        {
            var invitation = await _context.ManagerInvitations
                .FirstOrDefaultAsync(i => i.Id == invitationId && i.PropertyManagerId == managerId);

            if (invitation == null || invitation.Status != InvitationStatus.Pending)
                return null;

            invitation.Status = accept ? InvitationStatus.Accepted : InvitationStatus.Declined;
            invitation.RespondedAt = DateTime.UtcNow;

            ManagementAgreement? agreement = null;
            if (accept)
            {
                agreement = new ManagementAgreement
                {
                    PropertyId = invitation.PropertyId,
                    PropertyManagerId = managerId,
                    MonthlyFee = monthlyFee ?? 100,
                    StartDate = DateTime.UtcNow,
                    Status = AgreementStatus.Active,
                    Terms = terms
                };
                await _context.ManagementAgreements.AddAsync(agreement);
            }

            await _context.SaveChangesAsync();
            return agreement;
        }
    }
}