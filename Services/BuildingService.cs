using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class BuildingService : CachedService<Building>
    {
        public BuildingService(RoomRentalsContext context, IMemoryCache memoryCache) : base(memoryCache, context, "Buildings") { }
        /*/// <summary>
        /// Возвращает все объекты Building, хранящиеся в базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<List<Building>?> GetBuildings()
        {
            if (!_cache.TryGetValue("Buildings", out List<Building>? buildings))
            {
                buildings = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return buildings;
        }*/
        /// <summary>
        /// Возвращает объект Building
        /// </summary>
        /// <returns></returns>
        public override async Task<Building> Get(int? id)
        {
            /*if (!_cache.TryGetValue("Buildings", out List<Building>? buildings))
            {
                buildings = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }*/
            return (await GetAll()).Single(e => e.BuildingId == id);
        }
        /*/// <summary>
        /// Добавляет объект Building
        /// </summary>
        /// <returns></returns>
        public async Task AddBuilding(Building building)
        {
            await _context.AddAsync(building);
            await _context.SaveChangesAsync();
            await AddCache();
        }
        /// <summary>
        /// Обновляет объект Building
        /// </summary>
        /// <returns></returns>
        public async Task UpdateBuilding(Building building)
        {
            _context.Update(building);
            await _context.SaveChangesAsync();
            await AddCache();
        }*/
        /// <summary>
        /// Удаляет объект Building
        /// </summary>
        /// <returns></returns>
        public override async Task Delete(Building building)
        {
            var rooms = await _context.Rooms
                        .Where(r => building.BuildingId == r.BuildingId)
                        .Select(r => r.RoomId)
                        .ToListAsync();

            var rentals = await _context.Rentals.Where(r => rooms.Contains(r.RoomId)).ToListAsync();
            var invoices = await _context.Invoices.Where(i => rooms.Contains(i.RoomId)).ToListAsync();

            if (rentals != null)
            {
                _context.Rentals.RemoveRange(rentals);
            }
            if (invoices != null)
            {
                _context.Invoices.RemoveRange(invoices);
            }
            await _context.SaveChangesAsync();

            if (building != null)
            {
                _context.Buildings.Remove(building);
            }
            await _context.SaveChangesAsync();

            await UpdateCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        protected override async Task<List<Building>> UpdateCache()
        {
            // обращаемся к базе данных
            var buildings = await _context.Buildings.Include(b => b.OwnerOrganization).ToListAsync();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут

            if (buildings != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Buildings", buildings, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return buildings.ToList();
        }
    }
}
