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
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly RoomService _cache;
        private readonly BuildingService _buildingCache;
        private readonly RoomImageService _imageCache;
        private readonly int _pageSize = 10;
        private readonly IWebHostEnvironment _appEnvironment;

        public RoomsController(RoomService cache, BuildingService buildingCache, RoomImageService imageCache, IWebHostEnvironment appEnvironment, IConfiguration appConfig)
        {
            _cache = cache;
            _buildingCache = buildingCache;
            _imageCache = imageCache;
            _appEnvironment = appEnvironment;
            _pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }

        // GET: Rooms
        [SetSession("Room")]
        public async Task<IActionResult> Index(RoomFilterViewModel filterViewModel, int page = 1, RoomSortState sortOrder = RoomSortState.BuildingNameAsc)
        {
            if (HttpContext.Request.Method == "GET")
            {
                var dict = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Room");

                if (dict != null)
                {
                    filterViewModel.BuildingNameFind = dict["BuildingNameFind"];

                    Decimal areaFind;
                    if (dict.ContainsKey("AreaFind") && Decimal.TryParse(dict["AreaFind"], out areaFind))
                        filterViewModel.AreaFind = areaFind;
                    else
                        filterViewModel.AreaFind = null;
                }
            }

            var rooms = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(filterViewModel.BuildingNameFind))
                rooms = rooms.Where(e => e.Building.Name.Contains(filterViewModel.BuildingNameFind)).ToList();
            if (filterViewModel.AreaFind != null)
                rooms = rooms.Where(e => e.Area == filterViewModel.AreaFind).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case RoomSortState.BuildingNameAsc:
                    rooms = rooms.OrderBy(e => e.Building.Name).ToList();
                    break;
                case RoomSortState.BuildingNameDesc:
                    rooms = rooms.OrderByDescending(e => e.Building.Name).ToList();
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
                FilterViewModel = filterViewModel,
                SortViewModel = new RoomSortViewModel(sortOrder)
            };

            return View(roomsViewModel);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var room = await _cache.Get(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public async Task<IActionResult> Create()
        {
            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetAll(), "BuildingId", "Name");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,BuildingId,RoomNumber,Area,Description,Photos")] Room room)
        {
            if (ModelState.IsValid)
            {
                int? roomId = await _cache.Add(room);

                string[] paths = new string[room.Photos.Count()];
                for (int i = 0; i < paths.Length; i++)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Photos[i].FileName);

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\Rooms\\", fileName), FileMode.Create))
                    {
                        await room.Photos[i].CopyToAsync(fileStream);
                    }

                    await _imageCache.Add(new RoomImage()
                    {
                        ImagePath = Path.Combine("\\images\\Rooms\\", fileName),
                        RoomId = (int)roomId
                    });
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetAll(), "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var room = await _cache.Get(id);
            if (room == null)
            {
                return NotFound();
            }

            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetAll(), "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,BuildingId,RoomNumber,Area,Description,Photos")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _cache.Update(room);

                    await _imageCache.DeleteImageForRoom(room.RoomId);
                    string[] paths = new string[room.Photos.Count()];
                    for (int i = 0; i < paths.Length; i++)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Photos[i].FileName);

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\Rooms\\", fileName), FileMode.Create))
                        {
                            await room.Photos[i].CopyToAsync(fileStream);
                        }

                        await _imageCache.Add(new RoomImage()
                        {
                            ImagePath = Path.Combine("\\images\\Rooms\\", fileName),
                            RoomId = (int)room.RoomId
                        });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await RoomExistsAsync((int)room.RoomId)))
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
            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetAll(), "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var room = await _cache.Get(id);
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
            if (await _cache.GetAll() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Rooms'  is null.");
            }
            await _cache.Delete(await _cache.Get(id));
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoomExistsAsync(int id)
        {
            return ((await _cache.GetAll())?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
