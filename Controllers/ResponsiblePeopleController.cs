using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ResponsiblePeopleController : Controller
    {
        private readonly RoomRentalsContext _context;
        private readonly int _pageSize = 10;

        public ResponsiblePeopleController(RoomRentalsContext context)
        {
            _context = context;
        }

        // GET: ResponsiblePersons
        public async Task<IActionResult> Index(int page = 1, string surnameFind = "", string nameFind = "", string lastnameFind = "",
                                                PersonSortState sortOrder = PersonSortState.SurnameAsc)
        {
            var peopleQuery = await _context.ResponsiblePeople.ToListAsync();

            //Фильтрация
            if (!String.IsNullOrEmpty(surnameFind))
                peopleQuery = peopleQuery.Where(e => e.Surname.Contains(surnameFind)).ToList();
            if (!String.IsNullOrEmpty(nameFind))
                peopleQuery = peopleQuery.Where(e => e.Name.Contains(nameFind)).ToList();
            if (!String.IsNullOrEmpty(lastnameFind))
                peopleQuery = peopleQuery.Where(e => e.Lastname.Contains(lastnameFind)).ToList();

            //Сортировка
            switch (sortOrder)
            {
                case PersonSortState.SurnameAsc:
                    peopleQuery = peopleQuery.OrderBy(e => e.Surname).ToList();
                    break;
                case PersonSortState.SurnameDesc:
                    peopleQuery = peopleQuery.OrderByDescending(e => e.Surname).ToList();
                    break;
                case PersonSortState.NameAsc:
                    peopleQuery = peopleQuery.OrderBy(e => e.Name).ToList();
                    break;
                case PersonSortState.NameDesc:
                    peopleQuery = peopleQuery.OrderByDescending(e => e.Name).ToList();
                    break;
                case PersonSortState.LastnameAsc:
                    peopleQuery = peopleQuery.OrderBy(e => e.Lastname).ToList();
                    break;
                default:
                    peopleQuery = peopleQuery.OrderByDescending(e => e.Lastname).ToList();
                    break;
            }

            //Разбиение на страницы
            int count = peopleQuery.Count;
            peopleQuery = peopleQuery.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель отображения
            PeopleViewModel peopleViewModel = new PeopleViewModel()
            {
                People = peopleQuery,
                PageViewModel = new PageViewModel(page, count, _pageSize),
                FilterViewModel = new PersonFilterViewModel(surnameFind, nameFind, lastnameFind),
                SortViewModel = new PersonSortViewModel(sortOrder)
            };

            return View(peopleViewModel);
        }

        // GET: ResponsiblePersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ResponsiblePeople == null)
            {
                return NotFound();
            }

            var responsiblePerson = await _context.ResponsiblePeople
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (responsiblePerson == null)
            {
                return NotFound();
            }

            return View(responsiblePerson);
        }

        // GET: ResponsiblePersons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResponsiblePersons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Surname,Name,Lastname")] ResponsiblePerson responsiblePerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(responsiblePerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(responsiblePerson);
        }

        // GET: ResponsiblePersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ResponsiblePeople == null)
            {
                return NotFound();
            }

            var responsiblePerson = await _context.ResponsiblePeople.FindAsync(id);
            if (responsiblePerson == null)
            {
                return NotFound();
            }
            return View(responsiblePerson);
        }

        // POST: ResponsiblePersons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Surname,Name,Lastname")] ResponsiblePerson responsiblePerson)
        {
            if (id != responsiblePerson.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(responsiblePerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponsiblePersonExists(responsiblePerson.PersonId))
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
            return View(responsiblePerson);
        }

        // GET: ResponsiblePersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ResponsiblePeople == null)
            {
                return NotFound();
            }

            var responsiblePerson = await _context.ResponsiblePeople
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (responsiblePerson == null)
            {
                return NotFound();
            }

            return View(responsiblePerson);
        }

        // POST: ResponsiblePersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ResponsiblePeople == null)
            {
                return Problem("Entity set 'RoomRentalsContext.ResponsiblePeople'  is null.");
            }
            var responsiblePerson = await _context.ResponsiblePeople.FindAsync(id);
            if (responsiblePerson != null)
            {
                _context.ResponsiblePeople.Remove(responsiblePerson);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResponsiblePersonExists(int id)
        {
          return (_context.ResponsiblePeople?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
    }
}
