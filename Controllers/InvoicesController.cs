using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RoomRental.Data;
using RoomRental.Models;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortStates;
using RoomRental.ViewModels.SortViewModels;
using RoomRental.ViewModels;

namespace RoomRental.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly RoomRentalsContext _context;
        private readonly int _pageSize = 10;

        public InvoicesController(RoomRentalsContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(int page = 1, string organizationNameFind = "", string personFind = "", decimal? amountFind = null, DateTime? paymentDateFind = null, InvoiceSortState sortOrder = InvoiceSortState.OrganizationNameAsc)
        {
            var invoicesQuery = await _context.Invoices.ToListAsync();
            var organizationsQuery = await _context.Organizations.ToListAsync();
            var peopleQuery = await _context.ResponsiblePeople.ToListAsync();
            //Формирование осмысленных связей
            List<InvoiceViewModel> invoices = new List<InvoiceViewModel>();
            foreach (var item in invoicesQuery)
            {
                var organization = organizationsQuery.Single(e => e.OrganizationId == item.RentalOrganizationId);
                var room = _context.Rooms.FirstAsync(e => e.RoomId == item.RoomId);
                var person = peopleQuery.Single(e => e.PersonId == item.ResponsiblePerson);
                invoices.Add(new InvoiceViewModel(item.InvoiceId, organization.Name, room.Result.RoomId, item.Amount, item.PaymentDate,
                                person.Surname + " " + person.Name + " " + person.Lastname));
            }

            //Фильтрация
            if (!String.IsNullOrEmpty(organizationNameFind))
                invoices = invoices.Where(e => e.RentalOrganization.Contains(organizationNameFind)).ToList();
            if (!String.IsNullOrEmpty(personFind))
                invoices = invoices.Where(e => e.ResponsiblePerson.Contains(personFind)).ToList();
            if (amountFind != null)
                invoices = invoices.Where(e => e.Amount == amountFind).ToList();
            if (paymentDateFind != null)
                invoices = invoices.Where(e => e.PaymentDate == paymentDateFind).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case InvoiceSortState.OrganizationNameAsc:
                    invoices = invoices.OrderBy(e => e.RentalOrganization).ToList();
                    break;
                case InvoiceSortState.OrganizationNameDesc:
                    invoices = invoices.OrderByDescending(e => e.RentalOrganization).ToList();
                    break;
                case InvoiceSortState.PaymentDateAsc:
                    invoices = invoices.OrderBy(e => e.PaymentDate).ToList();
                    break;
                case InvoiceSortState.PaymentDateDesc:
                    invoices = invoices.OrderByDescending(e => e.PaymentDate).ToList();
                    break;
                case InvoiceSortState.AmountAsc:
                    invoices = invoices.OrderBy(e => e.Amount).ToList();
                    break;
                case InvoiceSortState.AmountDesc:
                    invoices = invoices.OrderByDescending(e => e.Amount).ToList();
                    break;
                case InvoiceSortState.ResponsiblePersonAsc:
                    invoices = invoices.OrderBy(e => e.ResponsiblePerson).ToList();
                    break;
                default:
                    invoices = invoices.OrderByDescending(e => e.ResponsiblePerson).ToList();
                    break;
            }

            //Разбиение на страницы
            int count = invoices.Count;
            invoices = invoices.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Формирование модели представления
            InvoicesViewModel invoicesViewModel = new InvoicesViewModel()
            {
                Invoices = invoices,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                FilterViewModel = new InvoiceFilterViewModel(organizationNameFind, paymentDateFind, amountFind, personFind),
                SortViewModel = new InvoiceSortViewModel(sortOrder)
            };

            return View(invoicesViewModel);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.RentalOrganization)
                .Include(i => i.ResponsiblePersonNavigation)
                .Include(i => i.Room)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name");

            var people = _context.ResponsiblePeople.Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceId,RentalOrganizationId,RoomId,Amount,PaymentDate,ResponsiblePerson")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", invoice.RentalOrganizationId);

            var people = _context.ResponsiblePeople.Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", invoice.RentalOrganizationId);

            var people = _context.ResponsiblePeople.Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,RentalOrganizationId,RoomId,Amount,PaymentDate,ResponsiblePerson")] Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", invoice.RentalOrganizationId);

            var people = _context.ResponsiblePeople.Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.RentalOrganization)
                .Include(i => i.ResponsiblePersonNavigation)
                .Include(i => i.Room)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
          return (_context.Invoices?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
