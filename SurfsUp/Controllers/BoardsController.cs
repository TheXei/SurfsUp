using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    public class BoardsController : Controller
    {
        private readonly SurfsUpContext _context;

        public BoardsController(SurfsUpContext context)
        {
            _context = context;
        }

        

        // GET: Boards
        public async Task<IActionResult> Index(string sortOrder,
                                                string currentFilter,
                                                string search,
                                                int? pageNumber,
                                                string type)
        {
            ViewData["CurrentSort"] = sortOrder;

            if (search != null)
            {
                pageNumber = 1;
            }
            else
            {
                search = currentFilter;
            }

            var boards = from m in _context.Board
                            select m;

            if (!String.IsNullOrEmpty(search) && !String.IsNullOrEmpty(type))
            {
                //boards = boards.AsEnumerable().Where(s => s.GetType().GetProperty(type).GetValue(s).ToString().ToLower().Contains(search.ToLower()));

                //var task = boards.where(b => b.GetType().GetProperty(type).GetValue(b, null));

                boards = type.ToLower() switch
                {
                    "name" => boards.Where(s => s.Name.ToLower()!.Contains(search.ToLower())),
                    "lenth" => boards.Where(s => s.Length.ToString().ToLower()!.Contains(search.ToLower())),
                    "thickness" => boards.Where(s => s.Thickness.ToString().ToLower()!.Contains(search.ToLower())),
                    "volume" => boards.Where(s => s.Volume.ToString().ToLower()!.Contains(search.ToLower())),
                    "type" => boards.Where(s => s.Type.ToString().ToLower()!.Contains(search.ToLower())),
                    "price" => boards.Where(s => s.Price.ToString().ToLower()!.Contains(search.ToLower())),
                    "equipments" => boards.Where(s => s.Equipments.ToLower()!.Contains(search.ToLower())),
                    _ => boards.Where(s => s.Name.ToLower()!.Contains(search.ToLower())),
                };
            }

            int pageSize = 4;
            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Boards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // GET: Boards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Boards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] Board board)
        {
            if (id != board.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.Id))
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
            return View(board);
        }

        // GET: Boards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Board == null)
            {
                return Problem("Entity set 'SurfsUpContext.Board'  is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
          return _context.Board.Any(e => e.Id == id);
        }
    }
}
