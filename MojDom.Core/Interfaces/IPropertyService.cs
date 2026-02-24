using MojDom.Core.Entities;

namespace MojDom.Core.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAllAsync(string? search, int? cityId, int? type);
        Task<Property?> GetByIdAsync(int id);
        Task<IEnumerable<Property>> GetByOwnerAsync(string ownerId);
        Task<Property> CreateAsync(Property property);
        Task<Property> UpdateAsync(Property property);
        Task DeleteAsync(int id);
    }
}