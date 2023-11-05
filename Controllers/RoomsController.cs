﻿using System;
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
        private readonly RoomImageService _imageCache;
        private readonly int _pageSize = 10;
        private readonly IWebHostEnvironment _appEnvironment;

        public RoomsController(RoomService cache, BuildingService buildingCache, RoomImageService imageCache, IWebHostEnvironment appEnvironment)
        {
            _cache = cache;
            _buildingCache = buildingCache;
            _imageCache = imageCache;
            _appEnvironment = appEnvironment;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(int page = 1, string buildingNameFind = "", decimal? areaFind = null, RoomSortState sortOrder = RoomSortState.BuildingNameAsc)
        {
            var roomsQuery = await _cache.GetRooms();
            var imagesQuery = await _imageCache.GetImages();
            var buildingsQuery = await _buildingCache.GetBuildings();
            //Формирование осмысленных связей
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            foreach (var item in roomsQuery)
            {
                var images = imagesQuery.Where(e => e.RoomId == item.RoomId).ToList();
                string[] paths = new string[images.Count()];
                for(int i = 0; i < paths.Length; i++ )
                {
                    paths[i] = images[i].ImagePath;
                }

                var building = buildingsQuery.Single(e => e.BuildingId == item.BuildingId);
                rooms.Add(new RoomViewModel((int)item.RoomId, building.Name, (decimal)item.Area, item.Description, paths /*item.Photo*/));
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
        public async Task<IActionResult> Create([Bind("RoomId,BuildingId,Area,Description,Photos")] Room room)
        {
            // Если ModelState не валидна, просмотрите ошибки
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    // Выведите ошибки или выполните другую логику обработки ошибок
                    Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                int? roomId = _cache.AddRoom(room).Result;

                string[] paths = new string[room.Photos.Count()];
                for (int i = 0; i < paths.Length; i++)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Photos[i].FileName);

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\Rooms\\", fileName), FileMode.Create))
                    {
                        await room.Photos[i].CopyToAsync(fileStream);
                    }

                    _imageCache.AddImage(new RoomImage()
                    {
                        ImagePath = Path.Combine("\\images\\Rooms\\", fileName),
                        RoomId = (int)roomId
                    });
                }

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
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,BuildingId,Area,Description,Photos")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string[] paths = new string[room.Photos.Count()];
                    for (int i = 0; i < paths.Length; i++)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Photos[i].FileName);

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\Rooms\\", fileName), FileMode.Create))
                        {
                            await room.Photos[i].CopyToAsync(fileStream);
                        }

                        _imageCache.AddImage(new RoomImage()
                        {
                            ImagePath = Path.Combine("\\images\\Rooms\\", fileName),
                            RoomId = (int)room.RoomId
                        });
                    }

                    _cache.UpdateRoom(room);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists((int)room.RoomId))
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
