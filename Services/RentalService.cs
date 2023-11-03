using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RentalService
    {
        private RoomRentalsContext _context;
        private IMemoryCache _cache;
        public RentalService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        /// <summary>
        /// Возвращает все объекты Rental, хранящиеся в базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<List<Rental>?> GetRentals()
        {
            if (!_cache.TryGetValue("Rentals", out List<Rental>? rentals))
            {
                rentals = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rentals;
        }
        /// <summary>
        /// Возвращает объект Rental
        /// </summary>
        /// <returns></returns>
        public async Task<Rental> GetRental(int? id)
        {
            if (!_cache.TryGetValue("Rentals", out List<Rental>? rentals))
            {
                rentals = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rentals.Single(e => e.RentalId == id);
        }
        /// <summary>
        /// Добавляет объект Rental
        /// </summary>
        /// <returns></returns>
        public async void AddRental(Rental rental)
        {
            _context.Add(rental);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Обновляет объект Rental
        /// </summary>
        /// <returns></returns>
        public async void UpdateRental(Rental rental)
        {
            _context.Update(rental);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Удаляет объект Rental
        /// </summary>
        /// <returns></returns>
        public async void DeleteRental(Rental rental)
        {
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
            }

            _context.SaveChanges();

            await AddCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        public async Task<List<Rental>?> AddCache()
        {
            _cache.Remove("Rentals");
            // обращаемся к базе данных
            var rentals = _context.Rentals.ToList();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут

            if (rentals != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Rentals", rentals, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return rentals.ToList();
        }
    }
}
