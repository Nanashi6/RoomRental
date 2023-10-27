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
    public class InvoicesController : Controller
    {
        private readonly RoomRentalsContext _context;

        public InvoicesController(RoomRentalsContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var roomRentalsContext = _context.Invoices.Include(i => i.RentalOrganization).Include(i => i.ResponsiblePersonNavigation).Include(i => i.Room);
            return View(await roomRentalsContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.RentalOrganization)
                .Include(i => i.ResponsiblePersonNavigation)
                .Include(i => i.Room)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationId");
            ViewData["ResponsiblePerson"] = new SelectList(_context.ResponsiblePeople, "PersonId", "PersonId");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceId,RentalOrganizationId,RoomId,Amount,PaymentDate,ResponsiblePerson")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationId", invoice.RentalOrganizationId);
            ViewData["ResponsiblePerson"] = new SelectList(_context.ResponsiblePeople, "PersonId", "PersonId", invoice.ResponsiblePerson);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationId", invoice.RentalOrganizationId);
            ViewData["ResponsiblePerson"] = new SelectList(_context.ResponsiblePeople, "PersonId", "PersonId", invoice.ResponsiblePerson);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,RentalOrganizationId,RoomId,Amount,PaymentDate,ResponsiblePerson")] Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
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
            ViewData["RentalOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationId", invoice.RentalOrganizationId);
            ViewData["ResponsiblePerson"] = new SelectList(_context.ResponsiblePeople, "PersonId", "PersonId", invoice.ResponsiblePerson);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", invoice.RoomId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.RentalOrganization)
                .Include(i => i.ResponsiblePersonNavigation)
                .Include(i => i.Room)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'RoomRentalsContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
          return (_context.Invoices?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
