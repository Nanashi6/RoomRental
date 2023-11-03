using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class InvoiceService
    {
        private RoomRentalsContext _context;
        private IMemoryCache _cache;
        public InvoiceService(RoomRentalsContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        /// <summary>
        /// Возвращает все объекты Invoice, хранящиеся в базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<List<Invoice>?> GetInvoices()
        {
            if (!_cache.TryGetValue("Invoices", out List<Invoice>? invoices))
            {
                invoices = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return invoices;
        }
        /// <summary>
        /// Возвращает объект Invoice
        /// </summary>
        /// <returns></returns>
        public async Task<Invoice> GetInvoice(int? id)
        {
            if (!_cache.TryGetValue("Invoices", out List<Invoice>? invoices))
            {
                invoices = AddCache().Result;
            }
            else
            {
                Console.WriteLine($"Список извлечен из кэша");
            }
            return invoices.Single(e => e.InvoiceId == id);
        }
        /// <summary>
        /// Добавляет объект Invoice
        /// </summary>
        /// <returns></returns>
        public async void AddInvoice(Invoice invoice)
        {
            _context.Add(invoice);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Обновляет объект Invoice
        /// </summary>
        /// <returns></returns>
        public async void UpdateInvoice(Invoice invoice)
        {
            _context.Update(invoice);
            _context.SaveChanges();
            await AddCache();
        }
        /// <summary>
        /// Удаляет объект Invoice
        /// </summary>
        /// <returns></returns>
        public async void DeleteInvoice(Invoice invoice)
        {
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            _context.SaveChanges();

            await AddCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        public async Task<List<Invoice>?> AddCache()
        {
            _cache.Remove("Invoices");
            // обращаемся к базе данных
            var invoices = _context.Invoices.ToList();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут

            if (invoices != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set("Invoices", invoices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return invoices.ToList();
        }
    }
}
