using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RentalService : CachedService<Rental>
    {
        public RentalService(RoomRentalsContext context, IMemoryCache memoryCache) : base(memoryCache, context, "Rentals") { }

        /*public async Task<List<Rental>?> GetRentals()
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
        }*/

        public async override Task<Rental> Get(int? id)
        {
            /*if (!_cache.TryGetValue("Rentals", out List<Rental>? rentals))
            {
                rentals = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }*/
            return (await GetAll()).Single(e => e.RentalId == id);
        }

        /*public async Task AddRental(Rental rental)
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
        }*/

        public override async Task Delete(Rental rental)
        {
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
            }

            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        protected override async Task<List<Rental>> UpdateCache()
        {
            var rentals = await _context.Rentals.Include(r => r.RentalOrganization).Include(r => r.Room).ToListAsync();

            if (rentals != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set(_name, rentals, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return rentals;
        }
    }
}