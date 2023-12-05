using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class OrganizationsController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly OrganizationService _cache;
        private readonly int _pageSize = 10;

        public OrganizationsController(OrganizationService cache, IHttpContextAccessor httpContextAccessor, IConfiguration appConfig)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }

        // GET: Organizations
        [HttpGet]
        public async Task<IActionResult> Index(OrganizationFilterViewModel filterViewModel, int page = 1, OrganizationSortState sortOrder = OrganizationSortState.NameAsc)
        {
            //Изъятие данных фильтрации из сессии
            var dict = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Organization");

            if (dict != null)
            {
                filterViewModel.OrganizationNameFind = dict["OrganizationNameFind"];
                filterViewModel.AddressFind = dict["AddressFind"];
            }

            var organizationsQuery = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(filterViewModel.OrganizationNameFind))
            {
                organizationsQuery = organizationsQuery.Where(e => e.Name.Contains(filterViewModel.OrganizationNameFind)).ToList();
            }
            if (!String.IsNullOrEmpty(filterViewModel.AddressFind))
            {
                organizationsQuery = organizationsQuery.Where(e => e.PostalAddress.Contains(filterViewModel.AddressFind)).ToList();
            }

            //Сортировка
            switch (sortOrder)
            {
                case OrganizationSortState.NameDesc:
                    organizationsQuery = organizationsQuery.OrderByDescending(e => e.Name).ToList();
                    break;
                case OrganizationSortState.AddressAsc:
                    organizationsQuery = organizationsQuery.OrderBy(e => e.PostalAddress).ToList();
                    break;
                case OrganizationSortState.AddressDesc:
                    organizationsQuery = organizationsQuery.OrderByDescending(e => e.PostalAddress).ToList();
                    break;
                default:
                    organizationsQuery = organizationsQuery.OrderBy(e => e.Name).ToList();
                    break;
            }

            //Разбиение на страницы
            int count = organizationsQuery.Count;
            organizationsQuery = organizationsQuery.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель отображения
            OrganizationsViewModel organizationsViewModel = new OrganizationsViewModel()
            {
                Organizations = organizationsQuery,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                FilterViewModel = filterViewModel,
                SortViewModel = new OrganizationSortViewModel(sortOrder)
            };

            return View(organizationsViewModel);
        }

        // GET: Organizations/Filter
        [HttpGet]
        [SetSession("Organization")]
        public async Task<IActionResult> Filter(OrganizationFilterViewModel filterViewModel = null, OrganizationSortState sortOrder = OrganizationSortState.NameAsc)
        {
            var routeValues = new RouteValueDictionary
            {
                { "filterViewModel", filterViewModel },
                { "sortOrder", sortOrder }
            };

            return RedirectToAction(nameof(Index), routeValues);
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var organization = await _cache.Get(id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationId,Name,PostalAddress")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                await _cache.Add(organization);
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var organization = await _cache.Get(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrganizationId,Name,PostalAddress")] Organization organization)
        {
            if (id != organization.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _cache.Update(organization);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await OrganizationExists(organization.OrganizationId)))
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
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _cache.GetAll() == null)
            {
                return NotFound();
            }

            var organization = await _cache.Get(id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _cache.GetAll() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Organizations' is null.");
            }

            await _cache.Delete(await _cache.Get(id));

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrganizationExists(int id)
        {
            return ((await _cache.GetAll())?.Any(e => e.OrganizationId == id)).GetValueOrDefault();
        }
    }
}
