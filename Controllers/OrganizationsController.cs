using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly OrganizationService _cache;
        private readonly int _pageSize = 10;

        public OrganizationsController(OrganizationService cache)
        {
            _cache = cache;
        }

        // GET: Organizations
        public async Task<IActionResult> Index(int page = 1, string organizationNameFind = "", OrganizationSortState sortOrder = OrganizationSortState.NameAsc)
        {
             var organizationsQuery = await _cache.GetOrganizations();
            //Фильтрация
            if (!String.IsNullOrEmpty(organizationNameFind))
                organizationsQuery = organizationsQuery.Where(e => e.Name.Contains(organizationNameFind)).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case OrganizationSortState.NameAsc:
                    organizationsQuery = organizationsQuery.OrderBy(e => e.Name).ToList();
                    break;
                case OrganizationSortState.NameDesc:
                    organizationsQuery = organizationsQuery.OrderByDescending(e => e.Name).ToList();
                    break;
                case OrganizationSortState.AddressAsc:
                    organizationsQuery = organizationsQuery.OrderBy(e => e.PostalAddress).ToList();
                    break;
                default:
                    organizationsQuery = organizationsQuery.OrderByDescending(e => e.PostalAddress).ToList();
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
            if (id == null || _cache.GetOrganizations() == null)
            {
                return NotFound();
            }

            var organization = _cache.GetOrganization(id).Result;
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
                _cache.AddOrganization(organization);
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _cache.GetOrganizations() == null)
            {
                return NotFound();
            }

            var organization = await _cache.GetOrganization(id);
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
                    _cache.UpdateOrganization(organization);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.OrganizationId))
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
            if (id == null || _cache.GetOrganizations() == null)
            {
                return NotFound();
            }

            var organization = await _cache.GetOrganization(id);
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
            if (_cache.GetOrganizations() == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Organizations' is null.");
            }

            _cache.DeleteOrganization(_cache.GetOrganization(id).Result);

            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return (_cache.GetOrganizations().Result?.Any(e => e.OrganizationId == id)).GetValueOrDefault();
        }
    }
}
