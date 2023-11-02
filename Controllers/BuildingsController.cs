using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRental.Data;
using RoomRental.Models;
using RoomRental.ViewModels;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortStates;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.Controllers
{
    [Authorize(Roles = "User")]
    public class BuildingsController : Controller
    {
        private readonly RoomRentalsContext _context;
        private readonly int _pageSize = 10;

        public BuildingsController(RoomRentalsContext context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index(int page = 1, string buildingNameFind = "", string organizationNameFind = "", string addressFind = "",
                                                int? floorsFind = null, BuildingSortState sortOrder = BuildingSortState.NameAsc)
        {
            var buildingsQuery = await _context.Buildings.ToListAsync();

            //Формирование осмысленных связей
            List<BuildingViewModel> buildings = new List<BuildingViewModel>();
            foreach (var item in buildingsQuery)
            {
                var organization = _context.Organizations.FirstAsync(e => e.OrganizationId == item.OwnerOrganizationId);
                buildings.Add(new BuildingViewModel(item.BuildingId, item.Name, organization.Result.Name, item.PostalAddress, item.Floors, item.Description, item.FloorPlan));
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
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .Include(b => b.OwnerOrganization)
                .FirstOrDefaultAsync(m => m.BuildingId == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            ViewData["OwnerOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name");
            return View();
        }

        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuildingId,Name,OwnerOrganizationId,PostalAddress,Floors,Description,FloorPlan")] BuildingBindModel building/*int BuildingId, string Name, int OwnerOrganizationId, string PostalAddress, int Floors, string Description, IFormFile FloorPlan*/)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Building()
                {
                    BuildingId = building.BuildingId,
                    Name = building.Name,
                    OwnerOrganizationId = building.OwnerOrganizationId,
                    PostalAddress = building.PostalAddress,
                    Floors = building.Floors,
                    Description = building.Description,
                    FloorPlan = ConvertIFormFileToByteArray(building.FloorPlan)
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            ViewData["OwnerOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // POST: Buildings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuildingId,Name,OwnerOrganizationId,PostalAddress,Floors,Description,FloorPlan")] BuildingBindModel building)
        {
            if (id != building.BuildingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Building()
                    {
                        BuildingId = building.BuildingId,
                        Name = building.Name,
                        OwnerOrganizationId = building.OwnerOrganizationId,
                        PostalAddress = building.PostalAddress,
                        Floors = building.Floors,
                        Description = building.Description,
                        FloorPlan = ConvertIFormFileToByteArray(building.FloorPlan)
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.BuildingId))
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
            ViewData["OwnerOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", building.OwnerOrganizationId);
            return View(building);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .Include(b => b.OwnerOrganization)
                .FirstOrDefaultAsync(m => m.BuildingId == id);
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
            if (_context.Buildings == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Buildings'  is null.");
            }
            var building = await _context.Buildings.FindAsync(id);

            var rooms = await _context.Rooms
                        .Where(r => building.BuildingId == r.BuildingId)
                        .Select(r => r.RoomId)
                        .ToListAsync();

            var rentals = await _context.Rentals.Where(r => rooms.Contains(r.RoomId)).ToListAsync();
            var invoices = await _context.Invoices.Where(i => rooms.Contains(i.RoomId)).ToListAsync();

            if (rentals != null)
            {
                _context.Rentals.RemoveRange(rentals);
            }
            if (invoices != null)
            {
                _context.Invoices.RemoveRange(invoices);
            }
            await _context.SaveChangesAsync();

            if (building != null)
            {
                _context.Buildings.Remove(building);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
            return (_context.Buildings?.Any(e => e.BuildingId == id)).GetValueOrDefault();
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
