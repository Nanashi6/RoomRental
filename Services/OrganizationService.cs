using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class OrganizationService
    {
        private RoomRentalsContext _context;
        private IMemoryCache _cache;
        public OrganizationService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<List<Organization>?> GetOrganizations()
        {
            if (!_cache.TryGetValue("Organizations", out List<Organization>? organizations))
            {
                organizations = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return organizations;
        }

        public async Task<Organization> GetOrganization(int? id)
        {
            if (!_cache.TryGetValue("Organizations", out List<Organization>? organizations))
            {
                organizations = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return organizations.Single(e => e.OrganizationId == id);
        }

        public async Task AddOrganization(Organization organization)
        {
            await _context.AddAsync(organization);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task UpdateOrganization(Organization organization)
        {
            _context.Update(organization);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task DeleteOrganization(Organization organization)
        {
            var rentals = await _context.Rentals.Where(e => e.RentalOrganizationId == organization.OrganizationId).ToListAsync();
            var invoices = await _context.Invoices.Where(e => e.RentalOrganizationId == organization.OrganizationId).ToListAsync();

            var rooms = await _context.Rooms
                        .Where(r => _context.Buildings
                            .Where(b => organization.OrganizationId == b.OwnerOrganizationId)
                            .Select(b => b.BuildingId)
                            .Contains((int)r.BuildingId))
                        .Select(r => r.RoomId)
                        .ToListAsync();

            rentals.AddRange(await _context.Rentals.Where(r => rooms.Contains(r.RoomId)).ToListAsync());
            invoices.AddRange(await _context.Invoices.Where(i => rooms.Contains(i.RoomId)).ToListAsync());

            if (rentals != null)
            {
                _context.Rentals.RemoveRange(rentals);
            }
            if (invoices != null)
            {
                _context.Invoices.RemoveRange(invoices);
            }
            await _context.SaveChangesAsync();

            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            await _context.SaveChangesAsync();

            await AddCache();
        }

        public async Task<List<Organization>?> AddCache()
        {
            //_cache.Remove("Organizations");
            // обращаемся к базе данных
            var organizations = await _context.Organizations.ToListAsync();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут
            if (organizations != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Organizations", organizations, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return organizations;
        }
    }
}
