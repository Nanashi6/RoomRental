using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class PeopleService : CachedService<ResponsiblePerson>
    {
        public PeopleService(RoomRentalsContext context, IMemoryCache memoryCache) : base(memoryCache, context, "People") { }

        /*public async Task<List<ResponsiblePerson>?> GetPeople()
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
        }*/

        public override async Task<ResponsiblePerson> Get(int? id)
        {
            /*if (!_cache.TryGetValue("People", out List<ResponsiblePerson>? people))
            {
                people = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }*/
            return (await GetAll()).Single(e => e.PersonId == id);
        }

        /*public async Task AddPerson(ResponsiblePerson person)
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
        }*/

        public override async Task Delete(ResponsiblePerson person)
        {
            if (person != null)
            {
                _context.ResponsiblePeople.Remove(person);
            }

            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        protected override async Task<List<ResponsiblePerson>> UpdateCache()
        {
            var people = await _context.ResponsiblePeople.ToListAsync();

            if (people != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set(_name, people, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return people;
        }
    }
}