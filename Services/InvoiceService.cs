using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Services
{
    public class InvoiceService : CachedService<Invoice>
    {
        public InvoiceService(RoomRentalsContext context, IMemoryCache memoryCache) : base(memoryCache, context, "Invoices") { }
        /// <summary>
        /// Возвращает объект Invoice
        /// </summary>
        /// <returns></returns>
        public override async Task<Invoice> Get(int? id)
        {
            return (await GetAll()).Single(e => e.InvoiceId == id);
        }
        /// <summary>
        /// Удаляет объект Invoice
        /// </summary>
        /// <returns></returns>
        public override async Task Delete(Invoice invoice)
        {
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();

            await UpdateCache();
        }
        /// <summary>
        /// Обновляет кэш
        /// </summary>
        /// <returns></returns>
        protected override async Task<List<Invoice>> UpdateCache()
        {
            // обращаемся к базе данных
            var invoices = await _context.Invoices.Include(i => i.Room).Include(i => i.RentalOrganization).Include(i => i.ResponsiblePerson).ToListAsync();
            // если пользователь найден, то добавляем в кэш - время кэширования 5 минут

            if (invoices != null)
            {
                Console.WriteLine($"Список извлечен из базы данных");
                _cache.Set(_name, invoices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return invoices;
        }
    }
}
