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
    public class RentalsController : Controller
    {
        private readonly RentalService _cache;
        private readonly OrganizationService _organizationCache;
        private readonly RoomService _roomCache;
        private readonly int _pageSize = 10;

        public RentalsController(RentalService cache, OrganizationService organizationCache, RoomService roomCache, IConfiguration appConfig)
        {
            _cache = cache;
            _organizationCache = organizationCache;
            _roomCache = roomCache;
            _pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }

        // GET: Rentals
        public async Task<IActionResult> Index(int page = 1, string organizationNameFind = "", DateTime? checkInDateFind = null,
                                                DateTime? checkOutDateFind = null, RentalSortState sortOrder = RentalSortState.OrganizationNameAsc)
        {
            var rentalsQuery = await _cache.GetRentals();
            var organizationsQuery = await _organizationCache.GetOrganizations();
            var roomQuery = await _roomCache.GetRooms();
            //Формирование осмысленных связей
            List<RentalViewModel> rentals = new List<RentalViewModel>();
            foreach (var item in rentalsQuery)
            {
                var organization = organizationsQuery.Single(e => e.OrganizationId == item.RentalOrganizationId);
                var room = roomQuery.Single(e => e.RoomId == item.RoomId);
                rentals.Add(new RentalViewModel(item.RentalId, (int)room.RoomId, organization.Name, item.CheckInDate, item.CheckOutDate));
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
            if (id == null || _cache.GetRentals() == null)
            {
                return NotFound();
            }

            var rental = await _cache.GetRental(id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["RentalOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name");
            ViewData["RoomId"] = new SelectList(_roomCache.GetRooms().Result, "RoomId", "RoomId");
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalId,RoomId,RentalOrganizationId,CheckInDate,CheckOutDate")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _cache.AddRental(rental);
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(_roomCache.GetRooms().Result, "RoomId", "RoomId", rental.RoomId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _cache.GetRentals() == null)
            {
                return NotFound();
            }

            var rental = await _cache.GetRental(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["RentalOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(_roomCache.GetRooms().Result, "RoomId", "RoomId", rental.RoomId);
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
                    _cache.UpdateRental(rental);
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
            ViewData["RentalOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(_roomCache.GetRooms().Result, "RoomId", "RoomId", rental.RoomId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _cache.GetRentals() == null)
            {
                return NotFound();
            }

            var rental = await _cache.GetRental(id);
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
            if (_cache.GetRentals() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Rentals'  is null.");
            }
            _cache.DeleteRental(_cache.GetRental(id).Result);
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return (_cache.GetRentals().Result?.Any(e => e.RentalId == id)).GetValueOrDefault();
        }
    }
}
