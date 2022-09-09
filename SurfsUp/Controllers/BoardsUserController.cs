using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace SurfsUp.Controllers
{
    public class BoardsUserController : Controller
    {
        private readonly SurfsUpContext _context;

        public BoardsUserController(SurfsUpContext context)
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

            boards = boards.Include(r => r.Rent);

            await boards.Where(board => board.Rent != null && board.Rent.EndRent < DateTime.Now).ForEachAsync(board => board.Rent = null);
            {
                await _context.SaveChangesAsync();
            }

            boards = boards.Where(board => board.Rent == null);

            if (!String.IsNullOrEmpty(search) && !String.IsNullOrEmpty(type))
            {
                //boards = boards.AsEnumerable().Where(s => s.GetType().GetProperty(type).GetValue(s).ToString().ToLower().Contains(search.ToLower()));

                //var task = boards.where(b => b.GetType().GetProperty(type).GetValue(b, null));

                boards = type.ToLower() switch
                {
                    "name" => boards.Where(s => s.Name.ToLower()!.Contains(search.ToLower())),
                    "length" => boards.Where(s => s.Length.ToString().ToLower()!.Contains(search.ToLower())),
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

        // GET: BoardsUser/Details/5
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

        public async Task<IActionResult> RentOut(int? id)
        {

            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var rent = new Rent();
            return View(rent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentOut(int id, [Bind(include: "StartRent,EndRent")] Rent rent)
        {
            if (!BoardExists(id))
            {
                return NotFound();
            }

            rent.BoardId = id;
            if (rent.StartRent > rent.EndRent)
            {
                ModelState.AddModelError("StartRent", "Start date must be before end date");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Add(rent);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(rent);
        }

        private bool BoardExists(int id)
        {
          return _context.Board.Any(e => e.Id == id);
        }
    }
}
