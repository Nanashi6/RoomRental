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
                organizations = AddCache().Result;
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
                organizations = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return organizations.Single(e => e.OrganizationId == id);
        }

        public async void AddOrganization(Organization organization)
        {
            _context.Add(organization);
            _context.SaveChanges();
            await AddCache();
        }

        public async void UpdateOrganization(Organization organization)
        {
            _context.Update(organization);
            _context.SaveChanges();
            await AddCache();
        }

        public async void DeleteOrganization(Organization organization)
        {
            var rentals = _context.Rentals.Where(e => e.RentalOrganizationId == organization.OrganizationId).ToList();
            var invoices = _context.Invoices.Where(e => e.RentalOrganizationId == organization.OrganizationId).ToList();

            var rooms = _context.Rooms
                        .Where(r => _context.Buildings
                            .Where(b => organization.OrganizationId == b.OwnerOrganizationId)
                            .Select(b => b.BuildingId)
                            .Contains((int)r.BuildingId))
                        .Select(r => r.RoomId)
                        .ToList();

            rentals.AddRange(_context.Rentals.Where(r => rooms.Contains(r.RoomId)).ToList());
            invoices.AddRange(_context.Invoices.Where(i => rooms.Contains(i.RoomId)).ToList());

            if (rentals != null)
            {
                _context.Rentals.RemoveRange(rentals);
            }
            if (invoices != null)
            {
                _context.Invoices.RemoveRange(invoices);
            }
            _context.SaveChanges();

            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }

            _context.SaveChanges();

            await AddCache();
        }

        public async Task<List<Organization>?> AddCache()
        {
            _cache.Remove("Organizations");
            // обращаемся к базе данных
            var organizations = _context.Organizations.ToList();
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
