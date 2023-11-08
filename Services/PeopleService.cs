using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class PeopleService
    {
        private readonly RoomRentalsContext _context;
        private readonly IMemoryCache _cache;

        public PeopleService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<List<ResponsiblePerson>?> GetPeople()
        {
            if (!_cache.TryGetValue("People", out List<ResponsiblePerson>? people))
            {
                people = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return people;
        }

        public async Task<ResponsiblePerson> GetPerson(int? id)
        {
            if (!_cache.TryGetValue("People", out List<ResponsiblePerson>? people))
            {
                people = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return people.Single(e => e.PersonId == id);
        }

        public async Task AddPerson(ResponsiblePerson person)
        {
            await _context.AddAsync(person);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task UpdatePerson(ResponsiblePerson person)
        {
            _context.Update(person);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task DeletePerson(ResponsiblePerson person)
        {
            if (person != null)
            {
                _context.ResponsiblePeople.Remove(person);
            }

            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task<List<ResponsiblePerson>?> AddCache()
        {
            var people = await _context.ResponsiblePeople.ToListAsync();

            if (people != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("People", people, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return people;
        }
    }
}