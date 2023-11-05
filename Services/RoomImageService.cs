using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class RoomImageService
    {
        private RoomRentalsContext _context;
        private IMemoryCache _cache;
        public RoomImageService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        /// <summary>
        /// Возвращает все объекты Building, хранящиеся в базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoomImage>?> GetImages()
        {
            if (!_cache.TryGetValue("Images", out List<RoomImage>? images))
            {
                images = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return images;
        }
        /// <summary>
        /// Возвращает объект Room
        /// </summary>
        /// <returns></returns>
        public async Task<RoomImage> GetImage(int? id)
        {
            if (!_cache.TryGetValue("Images", out List<RoomImage>? images))
            {
                images = AddCache().Result;
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
                images = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return images.Where(e => e.RoomId == roomId).ToList();
        }
        /// <summary>
        /// Добавляет объект Room
        /// </summary>
        /// <returns></returns>
        public async void AddImage(RoomImage image)
        {
            _context.Add(image);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Обновляет объект Room
        /// </summary>
        /// <returns></returns>
        public async void UpdateImage(RoomImage image)
        {
            _context.Update(image);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Удаляет объект Room
        /// </summary>
        /// <returns></returns>
        public async void DeleteImage(RoomImage image)
        {
            if (image != null)
            {
                _context.RoomImages.Remove(image);
            }

            _context.SaveChanges();

            await AddCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoomImage>?> AddCache()
        {
            _cache.Remove("Images");
            // обращаемся к базе данных
            var images = _context.RoomImages.ToList();

            if (images != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Images", images, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return images.ToList();
        }
    }
}
