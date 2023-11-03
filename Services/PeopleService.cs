using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class PeopleService
    {
        private RoomRentalsContext _context;
        private IMemoryCache _cache;
        public PeopleService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        /// <summary>
        /// Возвращает все объекты Rental, хранящиеся в базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResponsiblePerson>?> GetPeople()
        {
            if (!_cache.TryGetValue("People", out List<ResponsiblePerson>? people))
            {
                people = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return people;
        }
        /// <summary>
        /// Возвращает объект Rental
        /// </summary>
        /// <returns></returns>
        public async Task<ResponsiblePerson> GetPerson(int? id)
        {
            if (!_cache.TryGetValue("People", out List<ResponsiblePerson>? people))
            {
                people = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return people.Single(e => e.PersonId == id);
        }
        /// <summary>
        /// Добавляет объект Rental
        /// </summary>
        /// <returns></returns>
        public async void AddPerson(ResponsiblePerson person)
        {
            _context.Add(person);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Обновляет объект Rental
        /// </summary>
        /// <returns></returns>
        public async void UpdatePerson(ResponsiblePerson person)
        {
            _context.Update(person);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Удаляет объект Rental
        /// </summary>
        /// <returns></returns>
        public async void DeletePerson(ResponsiblePerson person)
        {
            if (person != null)
            {
                _context.ResponsiblePeople.Remove(person);
            }

            _context.SaveChanges();

            await AddCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResponsiblePerson>?> AddCache()
        {
            _cache.Remove("People");
            // обращаемся к базе данных
            var people = _context.ResponsiblePeople.ToList();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут

            if (people != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("People", people, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return people.ToList();
        }
    }
}
