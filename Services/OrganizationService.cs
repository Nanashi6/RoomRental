using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class OrganizationService : CachedService<Organization>
    {
        public OrganizationService(RoomRentalsContext context, IMemoryCache memoryCache) : base(memoryCache, context, "Organizations")
        {

        }

/*        public async override Task<List<Organization>> GetAll()
        {
            if (!_cache.TryGetValue("Organizations", out List<Organization>? organizations))
            {
                organizations = await UpdateCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return organizations;
        }*/

        public async override Task<Organization> Get(int? id)
        {
            /*if (!_cache.TryGetValue(_name, out List<Organization>? organizations))
            {
                organizations = await UpdateCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }*/
            return (await GetAll()).Single(e => e.OrganizationId == id);
        }

/*        public async override Task Add(Organization organization)
        {
            await _context.AddAsync(organization);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async override Task Update(Organization organization)
        {
            _context.Update(organization);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }*/

        public async override Task Delete(Organization organization)
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

            await UpdateCache();
        }

        protected async override Task<List<Organization>> UpdateCache()
        {
            //_cache.Remove("Organizations");
            // обращаемся к базе данных
            var organizations = await _context.Organizations.ToListAsync();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут
            if (organizations != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set(_name, organizations, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return organizations;
        }
    }
}
