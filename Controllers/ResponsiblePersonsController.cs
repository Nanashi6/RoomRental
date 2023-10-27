using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRental.Data;
using RoomRental.Models;

namespace RoomRental.Controllers
{
    public class ResponsiblePersonsController : Controller
    {
        private readonly RoomRentalsContext _context;

        public ResponsiblePersonsController(RoomRentalsContext context)
        {
            _context = context;
        }

        // GET: ResponsiblePersons
        public async Task<IActionResult> Index()
        {
              return _context.ResponsiblePeople != null ? 
                          View(await _context.ResponsiblePeople.ToListAsync()) :
                          Problem("Entity set 'RoomRentalsContext.ResponsiblePeople'  is null.");
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
