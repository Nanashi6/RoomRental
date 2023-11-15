using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RoomService : CachedService<Room>
    {
        public RoomService(RoomRentalsContext context, IMemoryCache memoryCache) : base(memoryCache, context, "Rooms") { }

        /*public async o Task<List<Room>?> GetRooms()
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
        }*/

        public async override Task<Room> Get(int? id)
        {
            /*if (!_cache.TryGetValue("Rooms", out List<Room>? rooms))
            {
                rooms = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }*/
            return (await GetAll()).Single(e => e.RoomId == id);
        }

        public override async Task<int?> Add(Room room)
        {
            EntityEntry<Room> entRoom = await _context.AddAsync(room);
            await _context.SaveChangesAsync();
            await UpdateCache();

            return entRoom.Entity.RoomId;
        }

        /*public async Task Update(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            await AddCache();
        }*/

        public async override Task Delete(Room room)
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
            await UpdateCache();
        }

        protected async override Task<List<Room>> UpdateCache()
        {
            var rooms = await _context.Rooms.Include(r => r.Building).Include(r => r.RoomImages).ToListAsync();

            if (rooms != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set(_name, rooms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return rooms;
        }
    }
}