using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRental.Attributes;
using RoomRental.Models;
using RoomRental.Services;
using RoomRental.ViewModels;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortStates;
using RoomRental.ViewModels.SortViewModels;

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
        [SetSession("Rental")]
        public async Task<IActionResult> Index(RentalFilterViewModel filterViewModel, int page = 1, RentalSortState sortOrder = RentalSortState.OrganizationNameAsc)
        {
            if (HttpContext.Request.Method == "GET")
            {
                var dict = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Rental");

                if (dict != null)
                {
                    filterViewModel.OrganizationNameFind = dict["OrganizationNameFind"];

                    DateTime checkInDateFind;
                    if (dict.ContainsKey("CheckOutDateFind") && DateTime.TryParse(dict["CheckOutDateFind"], out checkInDateFind))
                        filterViewModel.CheckOutDateFind = checkInDateFind;
                    else
                        filterViewModel.CheckOutDateFind = null;

                    DateTime checkOutDateFind;
                    if (dict.ContainsKey("CheckInDateFind") && DateTime.TryParse(dict["CheckInDateFind"], out checkOutDateFind))
                        filterViewModel.CheckInDateFind = checkOutDateFind;
                    else
                        filterViewModel.CheckInDateFind = null;
                }
            }

            var rentals = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(filterViewModel.OrganizationNameFind))
                rentals = rentals.Where(e => e.RentalOrganization.Name.Contains(filterViewModel.OrganizationNameFind)).ToList();
            if (filterViewModel.CheckInDateFind != null)
                rentals = rentals.Where(e => e.CheckInDate >= filterViewModel.CheckInDateFind).ToList();
            if (filterViewModel.CheckOutDateFind != null)
                rentals = rentals.Where(e => e.CheckOutDate <= filterViewModel.CheckOutDateFind).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case RentalSortState.OrganizationNameAsc:
                    rentals = rentals.OrderBy(e => e.RentalOrganization.Name).ToList();
                    break;
                case RentalSortState.OrganizationNameDesc:
                    rentals = rentals.OrderByDescending(e => e.RentalOrganization.Name).ToList();
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
            int count = rentals.Count();
            rentals = rentals.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Формирование модели представления
            RentalsViewModel rentalsViewModel = new RentalsViewModel()
            {
                Rentals = rentals,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                FilterViewModel = filterViewModel,
                SortViewModel = new RentalSortViewModel(sortOrder)
            };

            return View(rentalsViewModel);
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var rental = await _cache.Get(id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetAll(), "OrganizationId", "Name");
            ViewData["RoomId"] = new SelectList(await _roomCache.GetAll(), "RoomId", "RoomId");
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalId,RoomId,RentalOrganizationId,CheckInDate,CheckOutDate")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                await _cache.Add(rental);
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetAll(), "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(await _roomCache.GetAll(), "RoomId", "RoomId", rental.RoomId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var rental = await _cache.Get(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetAll(), "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(await _roomCache.GetAll(), "RoomId", "RoomId", rental.RoomId);
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
                    await _cache.Update(rental);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await RentalExistsAsync(rental.RentalId)))
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
            ViewData["RentalOrganizationId"] = new SelectList(await _organizationCache.GetAll(), "OrganizationId", "Name", rental.RentalOrganizationId);
            ViewData["RoomId"] = new SelectList(await _roomCache.GetAll(), "RoomId", "RoomId", rental.RoomId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var rental = await _cache.Get(id);
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
            if (await _cache.GetAll() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Rentals'  is null.");
            }
            await _cache.Delete(await _cache.Get(id));
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RentalExistsAsync(int id)
        {
            return ((await _cache.GetAll())?.Any(e => e.RentalId == id)).GetValueOrDefault();
        }
    }
}
