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
    public class RentalsController : Controller
    {
        private readonly RoomRentalsContext _context;
        private readonly int _pageSize = 10;

        public RentalsController(RoomRentalsContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index(int page = 1, string organizationNameFind = "", DateTime? checkInDateFind = null,
                                                DateTime? checkOutDateFind = null, RentalSortState sortOrder = RentalSortState.OrganizationNameAsc)
        {
            var rentalsQuery = await _context.Rentals.ToListAsync();
            var organizationsQuery = await _context.Organizations.ToListAsync();
            //Формирование осмысленных связей
            List<RentalViewModel> rentals = new List<RentalViewModel>();
            foreach (var item in rentalsQuery)
            {
                var organization = organizationsQuery.Single(e => e.OrganizationId == item.RentalOrganizationId);
                var room = _context.Rooms.FirstAsync(e => e.RoomId == item.RoomId);
                rentals.Add(new RentalViewModel(item.RentalId, room.Result.RoomId, organization.Name, item.CheckInDate, item.CheckOutDate));
            }

            //Фильтрация
            if (!String.IsNullOrEmpty(organizationNameFind))
                rentals = rentals.Where(e => e.RentalOrganization.Contains(organizationNameFind)).ToList();
            if (checkInDateFind != null)
                rentals = rentals.Where(e => e.CheckInDate >= checkInDateFind).ToList();
            if (checkOutDateFind != null)
                rentals = rentals.Where(e => e.CheckOutDate <= checkOutDateFind).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case RentalSortState.OrganizationNameAsc:
                    rentals = rentals.OrderBy(e => e.RentalOrganization).ToList();
                    break;
                case RentalSortState.OrganizationNameDesc:
                    rentals = rentals.OrderByDescending(e => e.RentalOrganization).ToList();
                    break;
                case RentalSortState.CheckInDateAsc:
                    rentals = rentals.OrderBy(e => e.CheckInDate).ToList();
                    break;
                case RentalSortState.CheckInDateDesc:
                    rentals = rentals.OrderByDescending(e => e.CheckInDate).ToList();
                    break;
                case RentalSortState.CheckOutDateAsc:
                    rentals = rentals.OrderBy(e => e.CheckOutDate).ToList();
                    break;
                default:
                    rentals = rentals.OrderByDescending(e => e.CheckOutDate).ToList();
                    break;
            }

            //Разбиение на страницы
            int count = rentals.Count;
            rentals = rentals.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Формирование модели представления
            RentalsViewModel rentalsViewModel = new RentalsViewModel()
            {
                Rentals = rentals,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                FilterViewModel = new RentalFilterViewModel(organizationNameFind, checkInDateFind, checkOutDateFind),
                SortViewModel = new RentalSortViewModel(sortOrder)
            };

            return View(rentalsViewModel);
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.RentalOrganization)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RentalId == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalId,RoomId,RentalOrganizationId,CheckInDate,CheckOutDate")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", rental.RoomId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", rental.RoomId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalId,RoomId,RentalOrganizationId,CheckInDate,CheckOutDate")] Rental rental)
        {
            if (id != rental.RentalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.RentalId))
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
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", rental.RoomId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.RentalOrganization)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RentalId == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rentals == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Rentals'  is null.");
            }
            var rental = await _context.Rentals.FindAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
          return (_context.Rentals?.Any(e => e.RentalId == id)).GetValueOrDefault();
        }
    }
}
