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
using System.Text.Json.Serialization;
using System.Text.Json;

namespace RoomRental.Controllers
{
    [Authorize]
    public class BuildingsController : Controller
    {
        private readonly BuildingService _cache;
        private readonly OrganizationService _organizationCache;
        private readonly RoomService _roomCache;
        private readonly RentalService _rentalCache;
        private readonly int _pageSize = 10;
        private readonly IWebHostEnvironment _appEnvironment;

        public BuildingsController(BuildingService cache, OrganizationService organizationCache, RoomService roomCache, RentalService rentalCache, IWebHostEnvironment appEnvironment, IConfiguration appConfig)
        {
            _cache = cache;
            _organizationCache = organizationCache;
            _roomCache = roomCache;
            _rentalCache = rentalCache;
            _appEnvironment = appEnvironment;
            _pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }

        // GET: Buildings
        [SetSession("Building")]
        public async Task<IActionResult> Index(BuildingFilterViewModel filterViewModel, int page = 1, BuildingSortState sortOrder = BuildingSortState.NameAsc)
        {
            if (HttpContext.Request.Method == "GET")
            {
                var dict = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Building");

                if (dict != null)
                {
                    filterViewModel.AddressFind = dict["AddressFind"];
                    filterViewModel.BuildingNameFind = dict["BuildingNameFind"];
                    filterViewModel.OrganizationNameFind = dict["OrganizationNameFind"];

                    int floorsFind;
                    if (dict.ContainsKey("FloorsFind") && int.TryParse(dict["FloorsFind"], out floorsFind))
                        filterViewModel.FloorsFind = floorsFind;
                    else
                        filterViewModel.FloorsFind = null;
                }
            }

            var buildings = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(filterViewModel.BuildingNameFind))
                buildings = buildings.Where(e => e.Name.Contains(filterViewModel.BuildingNameFind)).ToList();
            if (!String.IsNullOrEmpty(filterViewModel.AddressFind))
                buildings = buildings.Where(e => e.PostalAddress.Contains(filterViewModel.AddressFind)).ToList();
            if (!String.IsNullOrEmpty(filterViewModel.OrganizationNameFind))
                buildings = buildings.Where(e => e.OwnerOrganization.Name.Contains(filterViewModel.OrganizationNameFind)).ToList();
            if (filterViewModel.FloorsFind != null)
                buildings = buildings.Where(e => e.Floors == filterViewModel.FloorsFind).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case BuildingSortState.NameAsc:
                    buildings = buildings.OrderBy(e => e.Name).ToList();
                    break;
                case BuildingSortState.NameDesc:
                    buildings = buildings.OrderByDescending(e => e.Name).ToList();
                    break;
                case BuildingSortState.AddressAsc:
                    buildings = buildings.OrderBy(e => e.PostalAddress).ToList();
                    break;
                case BuildingSortState.AddressDesc:
                    buildings = buildings.OrderByDescending(e => e.PostalAddress).ToList();
                    break;
                case BuildingSortState.OrganizationNameAsc:
                    buildings = buildings.OrderBy(e => e.OwnerOrganization.Name).ToList();
                    break;
                case BuildingSortState.OrganizationNameDesc:
                    buildings = buildings.OrderByDescending(e => e.OwnerOrganization.Name).ToList();
                    break;
                case BuildingSortState.FloorsAsc:
                    buildings = buildings.OrderBy(e => e.Floors).ToList();
                    break;
                default:
                    buildings = buildings.OrderByDescending(e => e.Floors).ToList();
                    break;
            }

            //Разбиение на страницы
            int count = buildings.Count;
            buildings = buildings.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Формирование модели представления
            BuildingsViewModel buildingsViewModel = new BuildingsViewModel()
            {
                Buildings = buildings,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                FilterViewModel = filterViewModel,
                SortViewModel = new RoomSortrViewModel(sortOrder)
            };

            return View(buildingsViewModel);
        }

        // GET: Buildings/Details/5
        /*[SetSession("BuildingDetails")]*/
        public async Task<IActionResult> Details(int? id, DateTime? startDate = null, DateTime? endDate = null)
        {
            /*if (HttpContext.Request.Method == "GET")
            {
                var dict = Infrastructure.SessionExtensions.Get(HttpContext.Session, "BuildingDetails");

                if (dict != null)
                {
                    DateTime date;
                    if (dict.ContainsKey("startDate") && DateTime.TryParse(dict["startDate"], out date))
                        startDate = date;
                    else
                        startDate = null;

                    if (dict.ContainsKey("endDate") && DateTime.TryParse(dict["endDate"], out date))
                        endDate = date;
                    else
                        endDate = null;
                }
            }*/

            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var building = await _cache.Get(id);
            if (building == null)
            {
                return NotFound();
            }

            //Общая площадь здания
            decimal buildingArea = (await _roomCache.GetAll())
                .Where(r => r.BuildingId == id)
                .Sum(r => r.Area);

            //Арендованные за промежуток времени здания
            var rooms = (await _rentalCache.GetAll())
                .Where(r => r.Room.BuildingId == id)
                .Select(r => new { Room = r.Room, CheckInDate = r.CheckInDate, CheckOutDate = r.CheckOutDate })
                .ToList();

            if (startDate == null)
                startDate = rooms.Min(e => e.CheckInDate);
            if (endDate == null)
                endDate = rooms.Max(e => e.CheckOutDate);

            List<DateTime> days = new();

            List<DaysRentals> daysRentals = new();

            DateTime currentDate = startDate.Value;
            while (currentDate <= endDate.Value)
            {
                //Подсчёт процента аренды здания на число currentDate
                daysRentals.Add(new DaysRentals()
                {
                    Date = DateOnly.FromDateTime(currentDate),
                    Percentage = rooms
                                    .Where(r => currentDate >= r.CheckInDate && r.CheckOutDate >= currentDate)
                                    .Sum(r => r.Room.Area) * 100 / buildingArea
                });

                // Увеличение даты на один день
                currentDate = currentDate.AddDays(1);
            }

            ViewBag.DaysRentals = daysRentals;
            //Формирование графика с арендой помещений
            ViewBag.StartDate = startDate.Value;
            ViewBag.EndDate = endDate.Value;

            return View(building);
        }

        // GET: Buildings/GetRentedRooms
        public async Task<IActionResult> GetRentedRooms(int buildingId, DateTime? currentDate = null)
        {
            var rentals = (await _rentalCache.GetAll())
                                    .Where(r => (currentDate >= r.CheckInDate && r.CheckOutDate >= currentDate) && r.Room.BuildingId == buildingId)
                                    .ToArray();
            return Json(rentals);
        }

        // GET: Buildings/Create
        public async Task<IActionResult> Create()
        {
            ViewData["OwnerOrganizationId"] = new SelectList((await _organizationCache.GetAll()), "OrganizationId", "Name");
            return View();
        }

        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuildingId,Name,OwnerOrganizationId,PostalAddress,Floors,Description,FloorPlanImage")] Building building)
        {
            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(building.FloorPlanImage.FileName);

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\FloorPlans\\", fileName), FileMode.Create))
                {
                    await building.FloorPlanImage.CopyToAsync(fileStream);
                }

                building.FloorPlan = Path.Combine("\\images\\FloorPlans\\", fileName);
                await _cache.Add(building);

                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerOrganizationId"] = new SelectList(await _organizationCache.GetAll(), "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var building = await _cache.Get(id);
            if (building == null)
            {
                return NotFound();
            }
            ViewData["OwnerOrganizationId"] = new SelectList(await _organizationCache.GetAll(), "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // POST: Buildings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuildingId,Name,OwnerOrganizationId,PostalAddress,Floors,Description,FloorPlanImage")] Building building)
        {
            if (id != building.BuildingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(building.FloorPlanImage.FileName);

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\FloorPlans\\", fileName), FileMode.Create))
                    {
                        await building.FloorPlanImage.CopyToAsync(fileStream);
                    }

                    building.FloorPlan = Path.Combine("\\images\\FloorPlans\\", fileName);
                    await _cache.Update(building);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await BuildingExists(building.BuildingId)))
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
            ViewData["OwnerOrganizationId"] = new SelectList(await _organizationCache.GetAll(), "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var building = await _cache.Get(id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _cache.GetAll() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Buildings'  is null.");
            }

            await _cache.Delete(await _cache.Get(id));

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BuildingExists(int id)
        {
            return ((await _cache.GetAll())?.Any(e => e.BuildingId == id)).GetValueOrDefault();
        }
    }
}
