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
using Microsoft.AspNetCore.Authorization;
using RoomRental.Services;

namespace RoomRental.Controllers
{
    [Authorize(Roles = "User")]
    public class InvoicesController : Controller
    {
        private readonly InvoiceService _cache;
        private readonly OrganizationService _organizationCache;
        private readonly PeopleService _peopleCache;
        private readonly RoomService _roomCache;
        private readonly int _pageSize = 10;

        public InvoicesController(InvoiceService cache, PeopleService peopleCache, RoomService roomCache, OrganizationService organizationCache, IConfiguration appConfig)
        {
            _cache = cache;
            _organizationCache = organizationCache;
            _peopleCache = peopleCache;
            _roomCache = roomCache;
            _pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }

        // GET: Invoices
        public async Task<IActionResult> Index(InvoiceFilterViewModel filterViewModel, int page = 1, InvoiceSortState sortOrder = InvoiceSortState.OrganizationNameAsc)
        {
            var invoices/*Query*/ = await _cache.GetInvoices();
            /*var organizationsQuery = await _organizationCache.GetOrganizations();
            var peopleQuery = await _peopleCache.GetPeople();
            var roomsQuery = await _roomCache.GetRooms();
            //Формирование осмысленных связей
            List<InvoiceViewModel> invoices = new List<InvoiceViewModel>();
            foreach (var item in invoicesQuery)
            {
                var organization = organizationsQuery.Single(e => e.OrganizationId == item.RentalOrganizationId);
                var room = roomsQuery.Single(e => e.RoomId == item.RoomId);
                var person = peopleQuery.Single(e => e.PersonId == item.ResponsiblePerson);
                invoices.Add(new InvoiceViewModel(item.InvoiceId, organization.Name, room.RoomId, item.Amount, item.PaymentDate,
                                person.Surname + " " + person.Name + " " + person.Lastname));
            }*/

            //Фильтрация
            if (!String.IsNullOrEmpty(filterViewModel.OrganizationNameFind))
                invoices = invoices.Where(e => e.RentalOrganization.Name.Contains(filterViewModel.OrganizationNameFind)).ToList();
            if (!String.IsNullOrEmpty(filterViewModel.ResponsiblePersonFind))
                invoices = invoices.Where(e => $"{e.ResponsiblePersonNavigation.Surname} {e.ResponsiblePersonNavigation.Name} {e.ResponsiblePersonNavigation.Lastname}".Contains(filterViewModel.ResponsiblePersonFind)).ToList();
            if (filterViewModel.AmountFind != null)
                invoices = invoices.Where(e => e.Amount == filterViewModel.AmountFind).ToList();
            if (filterViewModel.PaymentDateFind != null)
                invoices = invoices.Where(e => e.PaymentDate == filterViewModel.PaymentDateFind).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case InvoiceSortState.OrganizationNameAsc:
                    invoices = invoices.OrderBy(e => e.RentalOrganization.Name).ToList();
                    break;
                case InvoiceSortState.OrganizationNameDesc:
                    invoices = invoices.OrderByDescending(e => e.RentalOrganization.Name).ToList();
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
                    invoices = invoices.OrderBy(e => e.ResponsiblePerson.ToString()).ToList();
                    break;
                default:
                    invoices = invoices.OrderByDescending(e => e.ResponsiblePerson.ToString()).ToList();
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
                FilterViewModel = filterViewModel,
                SortViewModel = new InvoiceSortViewModel(sortOrder)
            };

            return View(invoicesViewModel);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _cache.GetInvoices() == null)
            {
                return NotFound();
            }

            var invoice = await _cache.GetInvoice(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetOrganizations(), "OrganizationId", "Name");

            var people = (await _peopleCache.GetPeople()).Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL");
            ViewData["RoomId"] = new SelectList(await _roomCache.GetRooms(), "RoomId", "RoomId");
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceId,RentalOrganizationId,RoomId,Amount,PaymentDate,ResponsiblePerson")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _cache.AddInvoice(invoice);
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetOrganizations(), "OrganizationId", "Name", invoice.RentalOrganizationId);

            var people = (await _peopleCache.GetPeople()).Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL", invoice.ResponsiblePerson);
            ViewData["RoomId"] = new SelectList(await _roomCache.GetRooms(), "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _cache.GetInvoices() == null)
            {
                return NotFound();
            }

            var invoice = await _cache.GetInvoice(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetOrganizations(), "OrganizationId", "Name", invoice.RentalOrganizationId);

            var people = (await _peopleCache.GetPeople()).Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL", invoice.ResponsiblePerson);
            ViewData["RoomId"] = new SelectList(await _roomCache.GetRooms(), "RoomId", "RoomId", invoice.RoomId);
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
                    await _cache.UpdateInvoice(invoice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await InvoiceExists(invoice.InvoiceId)))
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
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetOrganizations(), "OrganizationId", "Name", invoice.RentalOrganizationId);

            var people = (await _peopleCache.GetPeople()).Select(e => new { PersonId = e.PersonId, SNL = e.Surname + " " + e.Name + " " + e.Lastname }).ToList();

            ViewData["ResponsiblePerson"] = new SelectList(people, "PersonId", "SNL", invoice.ResponsiblePerson);
            ViewData["RoomId"] = new SelectList(await _roomCache.GetRooms(), "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _cache.GetInvoices() == null)
            {
                return NotFound();
            }

            var invoice = await _cache.GetInvoice(id);
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
            if (await _cache.GetInvoices() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Invoices'  is null.");
            }
            await _cache.DeleteInvoice(await _cache.GetInvoice(id));
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> InvoiceExists(int id)
        {
          return ((await _cache.GetInvoices())?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
