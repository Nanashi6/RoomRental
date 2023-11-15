using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomRental.Attributes;
using RoomRental.Models;
using RoomRental.Services;
using RoomRental.ViewModels;
using RoomRental.ViewModels.SortStates;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.Controllers
{
    [Authorize(Roles = "User")]
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

        // GET/POST: Organizations
        [HttpGet]
        [HttpPost]
        [SetSession("Organization")]
        public async Task<IActionResult> Index(string organizationNameFind = "", int page = 1, OrganizationSortState sortOrder = OrganizationSortState.NameAsc)
        {
            if (HttpContext.Request.Method == "GET")
            {
                var dict = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Organization");

                if (dict != null)
                {
                    organizationNameFind = dict["organizationNameFind"];
                }
            }

            var organizationsQuery = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(organizationNameFind))
            {
                organizationsQuery = organizationsQuery.Where(e => e.Name.Contains(organizationNameFind)).ToList();
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
                OrganizationNameFind = organizationNameFind,
                SortViewModel = new OrganizationSortViewModel(sortOrder)
            };

            return View(organizationsViewModel);
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
