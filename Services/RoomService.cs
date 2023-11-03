using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RoomService
    {
        private RoomRentalsContext _context;
        private IMemoryCache _cache;
        public RoomService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        /// <summary>
        /// Возвращает все объекты Building, хранящиеся в базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<List<Room>?> GetRooms()
        {
            if (!_cache.TryGetValue("Rooms", out List<Room>? rooms))
            {
                rooms = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rooms;
        }
        /// <summary>
        /// Возвращает объект Room
        /// </summary>
        /// <returns></returns>
        public async Task<Room> GetRoom(int? id)
        {
            if (!_cache.TryGetValue("Rooms", out List<Room>? rooms))
            {
                rooms = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rooms.Single(e => e.RoomId == id);
        }
        /// <summary>
        /// Добавляет объект Room
        /// </summary>
        /// <returns></returns>
        public async void AddRoom(Room room)
        {
            _context.Add(room);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Обновляет объект Room
        /// </summary>
        /// <returns></returns>
        public async void UpdateRoom(Room room)
        {
            _context.Update(room);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Удаляет объект Room
        /// </summary>
        /// <returns></returns>
        public async void DeleteRoom(Room room)
        {
            var rentals = _context.Rentals.Where(e => e.RoomId == room.RoomId).ToList();
            var invoices = _context.Invoices.Where(e => e.RoomId == room.RoomId).ToList();

            if (rentals != null)
            {
                _context.Rentals.RemoveRange(rentals);
            }
            if (invoices != null)
            {
                _context.Invoices.RemoveRange(invoices);
            }
            _context.SaveChanges();

            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            _context.SaveChanges();

            await AddCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        public async Task<List<Room>?> AddCache()
        {
            _cache.Remove("Rooms");
            // обращаемся к базе данных
            var rooms = _context.Rooms.ToList();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут

            if (rooms != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Rooms", rooms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return rooms.ToList();
        }
    }
}
