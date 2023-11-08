using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RentalService
    {
        private readonly RoomRentalsContext _context;
        private readonly IMemoryCache _cache;

        public RentalService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<List<Rental>?> GetRentals()
        {
            if (!_cache.TryGetValue("Rentals", out List<Rental>? rentals))
            {
                rentals = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rentals;
        }

        public async Task<Rental> GetRental(int? id)
        {
            if (!_cache.TryGetValue("Rentals", out List<Rental>? rentals))
            {
                rentals = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rentals.Single(e => e.RentalId == id);
        }

        public async Task AddRental(Rental rental)
        {
            await _context.AddAsync(rental);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task UpdateRental(Rental rental)
        {
            _context.Update(rental);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task DeleteRental(Rental rental)
        {
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
            }

            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task<List<Rental>?> AddCache()
        {
            var rentals = await _context.Rentals.ToListAsync();

            if (rentals != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Rentals", rentals, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return rentals;
        }
    }
}