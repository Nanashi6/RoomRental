using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRental.Data;
using RoomRental.Models;
using RoomRental.Services;
using RoomRental.ViewModels;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortStates;
using RoomRental.ViewModels.SortViewModels;
using System.IO;

namespace RoomRental.Controllers
{
    [Authorize(Roles = "User")]
    public class BuildingsController : Controller
    {
        private readonly BuildingService _cache;
        private readonly OrganizationService _organizationCache;
        private readonly int _pageSize = 10;
        private readonly IWebHostEnvironment _appEnvironment;

        public BuildingsController(BuildingService cache, OrganizationService organizationCache, IWebHostEnvironment appEnvironment, IConfiguration appConfig)
        {
            _cache = cache;
            _organizationCache = organizationCache;
            _appEnvironment = appEnvironment;
            _pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }

        // GET: Buildings
        public async Task<IActionResult> Index(int page = 1, string buildingNameFind = "", string organizationNameFind = "", string addressFind = "",
                                                int? floorsFind = null, BuildingSortState sortOrder = BuildingSortState.NameAsc)
        {
            var buildingsQuery = await _cache.GetBuildings();
            var organizationsQuery = await _organizationCache.GetOrganizations();
            //Формирование осмысленных связей
            var buildings = new List<BuildingViewModel>();
            foreach (var item in buildingsQuery)
            {
                var organization = organizationsQuery.Single(e => e.OrganizationId == item.OwnerOrganizationId);
                buildings.Add(new BuildingViewModel((int)item.BuildingId, item.Name, organization.Name, item.PostalAddress, item.Floors, item.Description, item.FloorPlan));
            }

            //Фильтрация
            if (!String.IsNullOrEmpty(buildingNameFind))
                buildings = buildings.Where(e => e.Name.Contains(buildingNameFind)).ToList();
            if (!String.IsNullOrEmpty(addressFind))
                buildings = buildings.Where(e => e.PostalAddress.Contains(addressFind)).ToList();
            if (!String.IsNullOrEmpty(organizationNameFind))
                buildings = buildings.Where(e => e.OwnerOrganization.Contains(organizationNameFind)).ToList();
            if (floorsFind != null)
                buildings = buildings.Where(e => e.Floors == floorsFind).ToList();

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
                    buildings = buildings.OrderBy(e => e.OwnerOrganization).ToList();
                    break;
                case BuildingSortState.OrganizationNameDesc:
                    buildings = buildings.OrderByDescending(e => e.OwnerOrganization).ToList();
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
                FilterViewModel = new BuildingFilterViewModel(buildingNameFind, organizationNameFind, addressFind, floorsFind),
                SortViewModel = new RoomSortrViewModel(sortOrder)
            };

            return View(buildingsViewModel);
        }

        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _cache.GetBuildings() == null)
            {
                return NotFound();
            }

            var building = _cache.GetBuilding(id).Result;
            if (building == null)
            {
                return NotFound();
            }
            var organization = await _organizationCache.GetOrganization(building.OwnerOrganizationId);
            return View(new BuildingViewModel(building.BuildingId, building.Name, organization.Name, building.PostalAddress, building.Floors, building.Description, building.FloorPlan));
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            ViewData["OwnerOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name");
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
                _cache.AddBuilding(building);

                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _cache.GetBuildings() == null)
            {
                return NotFound();
            }

            var building = await _cache.GetBuilding(id);
            if (building == null)
            {
                return NotFound();
            }
            ViewData["OwnerOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name", building.OwnerOrganizationId);
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
                    _cache.UpdateBuilding(building);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists((int)building.BuildingId))
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
            ViewData["OwnerOrganizationId"] = new SelectList(_organizationCache.GetOrganizations().Result, "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _cache.GetBuildings() == null)
            {
                return NotFound();
            }

            var building = _cache.GetBuilding(id).Result;
            if (building == null)
            {
                return NotFound();
            }
            var organization = await _organizationCache.GetOrganization(building.OwnerOrganizationId);
            return View(new BuildingViewModel(building.BuildingId, building.Name, organization.Name, building.PostalAddress, building.Floors, building.Description, building.FloorPlan));
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_cache.GetBuildings() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Buildings'  is null.");
            }

            _cache.DeleteBuilding(_cache.GetBuilding(id).Result);

            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
            return (_cache.GetBuildings().Result?.Any(e => e.BuildingId == id)).GetValueOrDefault();
        }
        public byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
