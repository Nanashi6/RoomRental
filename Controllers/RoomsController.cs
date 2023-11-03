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
    public class RoomsController : Controller
    {
        private readonly RoomService _cache;
        private readonly BuildingService _buildingCache;
        private readonly int _pageSize = 10;

        public RoomsController(RoomService cache, BuildingService buildingCache)
        {
            _cache = cache;
            _buildingCache = buildingCache;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(int page = 1, string buildingNameFind = "", decimal? areaFind = null, RoomSortState sortOrder = RoomSortState.BuildingNameAsc)
        {
            var roomsQuery = await _cache.GetRooms();
            var buildingsQuery = await _buildingCache.GetBuildings();
            //Формирование осмысленных связей
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            foreach (var item in roomsQuery)
            {
                var building = buildingsQuery.Single(e => e.BuildingId == item.BuildingId);
                rooms.Add(new RoomViewModel(item.RoomId, building.Name, item.Area, item.Description, item.Photo));
            }

            //Фильтрация
            if (!String.IsNullOrEmpty(buildingNameFind))
                rooms = rooms.Where(e => e.Building.Contains(buildingNameFind)).ToList();
            if (areaFind != null)
                rooms = rooms.Where(e => e.Area == areaFind).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case RoomSortState.BuildingNameAsc:
                    rooms = rooms.OrderBy(e => e.Building).ToList();
                    break;
                case RoomSortState.BuildingNameDesc:
                    rooms = rooms.OrderByDescending(e => e.Building).ToList();
                    break;
                case RoomSortState.AreaAsc:
                    rooms = rooms.OrderBy(e => e.Area).ToList();
                    break;
                default:
                    rooms = rooms.OrderByDescending(e => e.Area).ToList();
                    break;
            }

            //Разбиение на страницы
            int count = rooms.Count;
            rooms = rooms.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Формирование модели представления
            RoomsViewModel roomsViewModel = new RoomsViewModel()
            {
                Rooms = rooms,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                FilterViewModel = new RoomFilterViewModel(buildingNameFind, areaFind),
                SortViewModel = new RoomSortViewModel(sortOrder)
            };

            return View(roomsViewModel);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _cache.GetRooms() == null)
            {
                return NotFound();
            }

            var room = await _cache.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["BuildingId"] = new SelectList(_buildingCache.GetBuildings().Result, "BuildingId", "Name");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,BuildingId,Area,Description,Photo")] RoomBindModel room)
        {
            if (ModelState.IsValid)
            {
                _cache.AddRoom(new Room()
                {
                    RoomId = room.RoomId,
                    BuildingId = room.BuildingId,
                    Area = room.Area,
                    Description = room.Description,
                    Photo = ConvertIFormFileToByteArray(room.Photo)
                });
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_buildingCache.GetBuildings().Result, "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _cache.GetRooms() == null)
            {
                return NotFound();
            }

            var room = await _cache.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(_buildingCache.GetBuildings().Result, "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,BuildingId,Area,Description,Photo")] RoomBindModel room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _cache.UpdateRoom(new Room()
                    {
                        RoomId = room.RoomId,
                        BuildingId = room.BuildingId,
                        Area = room.Area,
                        Description = room.Description,
                        Photo = ConvertIFormFileToByteArray(room.Photo)
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            ViewData["BuildingId"] = new SelectList(_buildingCache.GetBuildings().Result, "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _cache.GetRooms() == null)
            {
                return NotFound();
            }

            var room = await _cache.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_cache.GetRooms() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Rooms'  is null.");
            }
            _cache.DeleteRoom(_cache.GetRoom(id).Result);
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
          return (_cache.GetRooms().Result?.Any(e => e.RoomId == id)).GetValueOrDefault();
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
