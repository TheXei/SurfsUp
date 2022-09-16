using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    public class RentsController : Controller
    {
        private readonly SurfsUpContext _context;

        public RentsController(SurfsUpContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            var surfsUpContext = _context.Rent.Include(r => r.ApplicationUser).Include(r => r.Board);
            return View(await surfsUpContext.ToListAsync());
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rent == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent
                .Include(r => r.ApplicationUser)
                .Include(r => r.Board)
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // GET: Rents/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplcationUser, "Id", "Id");
            ViewData["BoardId"] = new SelectList(_context.Board, "Id", "ImageURL");
            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoardId,StartRent,EndRent,RowVersion,ApplicationUserId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplcationUser, "Id", "Id", rent.ApplicationUserId);
            ViewData["BoardId"] = new SelectList(_context.Board, "Id", "ImageURL", rent.BoardId);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rent == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplcationUser, "Id", "Id", rent.ApplicationUserId);
            ViewData["BoardId"] = new SelectList(_context.Board, "Id", "ImageURL", rent.BoardId);
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BoardId,StartRent,EndRent,RowVersion,ApplicationUserId")] Rent rent)
        {
            if (id != rent.BoardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.BoardId))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplcationUser, "Id", "Id", rent.ApplicationUserId);
            ViewData["BoardId"] = new SelectList(_context.Board, "Id", "ImageURL", rent.BoardId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rent == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent
                .Include(r => r.ApplicationUser)
                .Include(r => r.Board)
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rent == null)
            {
                return Problem("Entity set 'SurfsUpContext.Rent'  is null.");
            }
            var rent = await _context.Rent.FindAsync(id);
            if (rent != null)
            {
                _context.Rent.Remove(rent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
          return _context.Rent.Any(e => e.BoardId == id);
        }
    }
}
