using MojDom.Core.Entities;

namespace MojDom.Core.Interfaces
{
    public interface IPropertyManagerService
    {
        Task<IEnumerable<PropertyManager>> GetAllAsync(string? search, string? zone);
        Task<PropertyManager?> GetByIdAsync(int id);
        Task<PropertyManager?> GetByUserIdAsync(string userId);
        Task<PropertyManager> UpdateAsync(PropertyManager manager);
        Task<ManagerInvitation> SendInvitationAsync(ManagerInvitation invitation);
        Task<IEnumerable<ManagerInvitation>> GetInvitationsForManagerAsync(int managerId);
        Task<IEnumerable<ManagerInvitation>> GetSentInvitationsAsync(string ownerId);
        Task<ManagementAgreement?> RespondToInvitationAsync(int invitationId, int managerId, bool accept, decimal? monthlyFee, string? terms);
    }
}