using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RoomImageService
    {
        private readonly RoomRentalsContext _context;
        private readonly IMemoryCache _cache;

        public RoomImageService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<List<RoomImage>?> GetImages()
        {
            if (!_cache.TryGetValue("Images", out List<RoomImage>? images))
            {
                images = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return images;
        }

        public async Task<RoomImage> GetImage(int? id)
        {
            if (!_cache.TryGetValue("Images", out List<RoomImage>? images))
            {
                images = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return images.Single(e => e.RoomId == id);
        }

        public async Task<List<RoomImage>> GetImageForRoom(int? roomId)
        {
            if (!_cache.TryGetValue("Images", out List<RoomImage>? images))
            {
                images = await AddCache();
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return images.Where(e => e.RoomId == roomId).ToList();
        }

        public async Task AddImage(RoomImage image)
        {
            await _context.AddAsync(image);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task UpdateImage(RoomImage image)
        {
            _context.Update(image);
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task DeleteImage(RoomImage image)
        {
            if (image != null)
            {
                _context.RoomImages.Remove(image);
            }

            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task DeleteImageForRoom(int roomId)
        {
            _context.RemoveRange(_context.RoomImages.Where(e => roomId == e.RoomId));
            await _context.SaveChangesAsync();
            await AddCache();
        }

        public async Task<List<RoomImage>?> AddCache()
        {
            var images = await _context.RoomImages.Include(r => r.Room).ToListAsync();

            if (images != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Images", images, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return images;
        }
    }
}