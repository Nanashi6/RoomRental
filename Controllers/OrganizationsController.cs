using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRental.Data;
using RoomRental.Models;
using RoomRental.ViewModels;
using RoomRental.ViewModels.SortStates;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly RoomRentalsContext _context;
        private readonly int _pageSize = 10;

        public OrganizationsController(RoomRentalsContext context)
        {
            _context = context;
        }

        // GET: Organizations
        public async Task<IActionResult> Index(int page = 1, string organizationNameFind = "", OrganizationSortState sortOrder = OrganizationSortState.NameAsc)
        {
            var organizationsQuery = await _context.Organizations.ToListAsync();

            //Фильтрация
            if(!String.IsNullOrEmpty(organizationNameFind))
                organizationsQuery = organizationsQuery.Where(e => e.Name.Contains(organizationNameFind)).ToList();

            //Сортировка
            switch(sortOrder)
            {
                case OrganizationSortState.NameAsc:
                    organizationsQuery = organizationsQuery.OrderBy(e => e.Name).ToList();
                    break;
                case OrganizationSortState.NameDesc:
                    organizationsQuery = organizationsQuery.OrderByDescending(e => e.Name).ToList();
                    break;
                case OrganizationSortState.AddressAsc:
                    organizationsQuery = organizationsQuery.OrderBy(e => e.PostalAddress).ToList();
                    break;
                default:
                    organizationsQuery = organizationsQuery.OrderByDescending(e => e.PostalAddress).ToList();
                    break;
            }

            //Разбиение на страницы
            int count = organizationsQuery.Count;
            organizationsQuery = organizationsQuery.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель отображения
            OrganizationsViewModel organizationsViewModel = new OrganizationsViewModel()
            {
                Organizations = organizationsQuery,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                OrganizationNameFind = organizationNameFind,
                SortViewModel = new OrganizationSortViewModel(sortOrder)
            };

            return View(organizationsViewModel);
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationId,Name,PostalAddress")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrganizationId,Name,PostalAddress")] Organization organization)
        {
            if (id != organization.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.OrganizationId))
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
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Organizations == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Organizations'  is null.");
            }
            var organization = await _context.Organizations.FindAsync(id);

            var rentals = await _context.Rentals.Where(e => e.RentalOrganizationId == organization.OrganizationId).ToListAsync();
            var invoices = await _context.Invoices.Where(e => e.RentalOrganizationId == organization.OrganizationId).ToListAsync();

            var rooms = await _context.Rooms
                        .Where(r => _context.Buildings
                            .Where(b => organization.OrganizationId == b.OwnerOrganizationId)
                            .Select(b => b.BuildingId)
                            .Contains(r.BuildingId))
                        .Select(r => r.RoomId)
                        .ToListAsync();

            rentals.AddRange(await _context.Rentals.Where(r => rooms.Contains(r.RoomId)).ToListAsync());
            invoices.AddRange(await _context.Invoices.Where(i => rooms.Contains(i.RoomId)).ToListAsync());

            if (rentals != null)
            {
                _context.Rentals.RemoveRange(rentals);
            }
            if (invoices != null)
            {
                _context.Invoices.RemoveRange(invoices);
            }
            await _context.SaveChangesAsync();

            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
          return (_context.Organizations?.Any(e => e.OrganizationId == id)).GetValueOrDefault();
        }
    }
}
