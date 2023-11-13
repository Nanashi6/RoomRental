using Microsoft.EntityFrameworkCore;
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
                invoices = await AddCache();
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
                invoices = await AddCache();
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
        public async Task AddInvoice(Invoice invoice)
        {
            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            await AddCache();
        }
        /// <summary>
        /// Обновляет объект Invoice
        /// </summary>
        /// <returns></returns>
        public async Task UpdateInvoice(Invoice invoice)
        {
            _context.Update(invoice);
            await _context.SaveChangesAsync();
            await AddCache();
        }
        /// <summary>
        /// Удаляет объект Invoice
        /// </summary>
        /// <returns></returns>
        public async Task DeleteInvoice(Invoice invoice)
        {
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();

            await AddCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        public async Task<List<Invoice>?> AddCache()
        {
            // обращаемся к базе данных
            var invoices = await _context.Invoices.Include(i => i.RentalOrganization).Include(i => i.ResponsiblePerson).Include(i => i.Room).ToListAsync();
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
