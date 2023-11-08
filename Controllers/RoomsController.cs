using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRental.Models;
using RoomRental.Services;
using RoomRental.ViewModels;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortStates;
using RoomRental.ViewModels.SortViewModels;

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

        public RoomsController(RoomService cache, BuildingService buildingCache, RoomImageService imageCache, IWebHostEnvironment appEnvironment, IConfiguration appConfig)
        {
            _cache = cache;
            _buildingCache = buildingCache;
            _imageCache = imageCache;
            _appEnvironment = appEnvironment;
            _pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }

        // GET: Rooms
        public async Task<IActionResult> Index(int page = 1, string buildingNameFind = "", decimal? areaFind = null, RoomSortState sortOrder = RoomSortState.BuildingNameAsc)
        {
            var rooms/*Query */= await _cache.GetRooms();
            /*var imagesQuery = await _imageCache.GetImages();
            var buildingsQuery = await _buildingCache.GetBuildings();
            //Формирование осмысленных связей
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            foreach (var item in roomsQuery)
            {
                var images = imagesQuery.Where(e => e.RoomId == item.RoomId).ToList();
                string[] paths = new string[images.Count()];
                for (int i = 0; i < paths.Length; i++)
                {
                    paths[i] = images[i].ImagePath;
                }

                var building = buildingsQuery.Single(e => e.BuildingId == item.BuildingId);
                rooms.Add(new RoomViewModel(item.RoomId, building.Name, item.Area, item.Description, paths));
            }*/

            //Фильтрация
            if (!String.IsNullOrEmpty(buildingNameFind))
                rooms = rooms.Where(e => e.Building.Name.Contains(buildingNameFind)).ToList();
            if (areaFind != null)
                rooms = rooms.Where(e => e.Area == areaFind).ToList();

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
                FilterViewModel = new RoomFilterViewModel(buildingNameFind, areaFind),
                SortViewModel = new RoomSortViewModel(sortOrder)
            };

            return View(roomsViewModel);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _cache.GetRooms() == null)
            {
                return NotFound();
            }

            var room = await _cache.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }
            var images = await _imageCache.GetImageForRoom(id);
            var building = await _buildingCache.GetBuilding(room.BuildingId);

            string[] paths = new string[images.Count()];
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = images[i].ImagePath;
            }

            return View(new RoomViewModel((int)room.RoomId, building.Name, (decimal)room.Area, room.Description, paths));
        }

        // GET: Rooms/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetBuildings(), "BuildingId", "Name");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,BuildingId,Area,Description,Photos")] Room room)
        {
            if (ModelState.IsValid)
            {
                int? roomId = await _cache.AddRoom(room);

                string[] paths = new string[room.Photos.Count()];
                for (int i = 0; i < paths.Length; i++)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Photos[i].FileName);

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\Rooms\\", fileName), FileMode.Create))
                    {
                        await room.Photos[i].CopyToAsync(fileStream);
                    }

                    await _imageCache.AddImage(new RoomImage()
                    {
                        ImagePath = Path.Combine("\\images\\Rooms\\", fileName),
                        RoomId = (int)roomId
                    });
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetBuildings(), "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _cache.GetRooms() == null)
            {
                return NotFound();
            }

            var room = await _cache.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }

            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetBuildings(), "BuildingId", "Name", room.BuildingId);
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
                    await _imageCache.DeleteImageForRoom(room.RoomId);
                    string[] paths = new string[room.Photos.Count()];
                    for (int i = 0; i < paths.Length; i++)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Photos[i].FileName);

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + Path.Combine("\\images\\Rooms\\", fileName), FileMode.Create))
                        {
                            await room.Photos[i].CopyToAsync(fileStream);
                        }

                        await _imageCache.AddImage(new RoomImage()
                        {
                            ImagePath = Path.Combine("\\images\\Rooms\\", fileName),
                            RoomId = (int)room.RoomId
                        });
                    }

                    await _cache.UpdateRoom(room);
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
            ViewData["BuildingId"] = new SelectList(await _buildingCache.GetBuildings(), "BuildingId", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _cache.GetRooms() == null)
            {
                return NotFound();
            }

            var room = await _cache.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }
            var images = await _imageCache.GetImageForRoom(id);
            var building = await _buildingCache.GetBuilding(room.BuildingId);

            string[] paths = new string[images.Count()];
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = images[i].ImagePath;
            }

            return View(new RoomViewModel(room.RoomId, building.Name, room.Area, room.Description, paths));
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _cache.GetRooms() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Rooms'  is null.");
            }
            await _cache.DeleteRoom(await _cache.GetRoom(id));
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoomExistsAsync(int id)
        {
            return ((await _cache.GetRooms())?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
