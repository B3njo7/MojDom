using Microsoft.EntityFrameworkCore;
using MojDom.Core.Entities;
using MojDom.Core.Interfaces;
using MojDom.Infrastructure.Data;

namespace MojDom.Infrastructure.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Property>> GetAllAsync(string? search, int? cityId, int? type)
        {
            var query = _context.Properties
                .Include(p => p.City)
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search) || p.Address.Contains(search));
            if (cityId.HasValue)
                query = query.Where(p => p.CityId == cityId);
            if (type.HasValue)
                query = query.Where(p => (int)p.Type == type);

            return await query.ToListAsync();
        }

        public async Task<Property?> GetByIdAsync(int id)
            => await _context.Properties
                .Include(p => p.City)
                .Include(p => p.Images)
                .Include(p => p.Owner)
                .Include(p => p.Agreements)
                    .ThenInclude(a => a.PropertyManager)
                        .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Property>> GetByOwnerAsync(string ownerId)
            => await _context.Properties
                .Include(p => p.City)
                .Include(p => p.Images)
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();

        public async Task<Property> CreateAsync(Property property)
        {
            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<Property> UpdateAsync(Property property)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task DeleteAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
            }
        }
    }
}