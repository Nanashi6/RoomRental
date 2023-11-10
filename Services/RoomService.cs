using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RoomService
    {
        private readonly RoomRentalsContext _context;
        private readonly IMemoryCache _cache;

        public RoomService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<List<Room>?> GetRooms()
        {
            if (!_cache.TryGetValue("Rooms", out List<Room>? rooms))
            {
                rooms = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rooms;
        }

        public async Task<Room> GetRoom(int? id)
        {
            if (!_cache.TryGetValue("Rooms", out List<Room>? rooms))
            {
                rooms = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return rooms.Single(e => e.RoomId == id);
        }

        public async Task<int?> AddRoom(Room room)
        {
            EntityEntry<Room> entRoom = await _context.AddAsync(room);
            await _context.SaveChangesAsync();
            await AddCache();

            return entRoom.Entity.RoomId;
        }

        public async Task UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task DeleteRoom(Room room)
        {
            var rentals = await _context.Rentals.Where(e => e.RoomId == room.RoomId).ToListAsync();
            var invoices = await _context.Invoices.Where(e => e.RoomId == room.RoomId).ToListAsync();

            if (rentals != null)
            {
                _context.Rentals.RemoveRange(rentals);
            }
            if (invoices != null)
            {
                _context.Invoices.RemoveRange(invoices);
            }
            await _context.SaveChangesAsync();

            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task<List<Room>?> AddCache()
        {
            var rooms = await _context.Rooms.Include(r => r.Building).Include(r => r.RoomImages).ToListAsync(); /*.Include(r => r.Building)*/

            if (rooms != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Rooms", rooms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return rooms;
        }
    }
}