using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    /* A way to restrict access to the controller to only users with the role of Admin. */
    [Authorize(Roles = StaticDetails.Role_User_Admin)]
    public class BoardsController : Controller
    {
        private readonly SurfsUpContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BoardsController(SurfsUpContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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

            /* Filtering the boards by the search string and then sorting them by the type. */
            if (!String.IsNullOrEmpty(search))
                boards = from b in boards where b.Name.ToLower()!.Contains(search.ToLower()) select b;

            if (!String.IsNullOrEmpty(type))
            {
                //PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(Board)).Find("Length", true);
                //var test2 = from b in _context.Board.ToList() orderby prop.GetValue(b) select b; //THIS WORKS
                //ReturnedList = (from b in _context.Board.ToList() orderby prop.GetValue(b) select b).ToList(); ///Works too
                boards = type.ToLower() switch
                {
                    "name" => from b in boards orderby b.Name select b,
                    "length" => from b in boards orderby b.Length select b,
                    "thickness" => from b in boards orderby b.Thickness select b,
                    "volume" => from b in boards orderby b.Volume select b,
                    "type" => from b in boards orderby b.Type select b,
                    "price" => from b in boards orderby b.Price select b,
                    //"equipments" => from b in boards orderby b.Equipments select b,
                    _ => from b in boards orderby b.Name select b,
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
        public async Task<IActionResult> Create([Bind("Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] Board board, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(wwwRootPath, @"images\boards");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    board.ImageURL = @"\images\boards\" + fileName + extension;
                }

                if (board.Type == BoardType.SUP)
                {
                    SUP newSUP = new()
                    {
                        Id = board.Id,
                        Name = board.Name,
                        Length = board.Length,
                        Width = board.Width,
                        Thickness = board.Thickness,
                        Volume = board.Volume,
                        Type = board.Type,
                        Price = board.Price,
                        ImageURL = board.ImageURL,
                        Equipment = "Paddle, Fin"
                    };
                    _context.Add(newSUP);
                }
                else
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
