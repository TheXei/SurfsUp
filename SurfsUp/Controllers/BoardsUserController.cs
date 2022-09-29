﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SurfsUp.Controllers
{
    public class BoardsUserController : Controller
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;

        public BoardsUserController(SurfsUpContext context, SurfsUpIdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        //public Task SortAndSearch(IQueryable<Board> boardList, string type, string search)
        //{
        //    //IQueryable<Board> boards = boardList;


        //    return Task.CompletedTask;
        //}

        // GET: Boards
        public async Task<IActionResult> Index(string currentFilter,
                                                string search,
                                                int? pageNumber,
                                                string type,
                                                bool rentError)
        {
            ViewData["CurrentFilter"] = search;
            ViewData["Type"] = type;
            
            if (rentError)
                ViewData["Locked"] = "Someone else is currently renting this Board. Please try again later.";
            
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
                    "equipments" => from b in boards orderby b.Equipments select b,
                    _ => from b in boards orderby b.Name select b,
                };
            }

            int pageSize = 8;
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

        //method that compares to long values
        

        //GET MOETHOD
        public async Task<IActionResult> RentOut(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }
            var boardToUpdate = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);
            TimeSpan isTimerOver = DateTime.Now - boardToUpdate.LockDate;
            if (isTimerOver.TotalMinutes < 5)
            {
                return RedirectToAction(nameof(Index), new { @rentError = true });
            }
            boardToUpdate.LockDate = DateTime.Now;
            await _context.SaveChangesAsync();
            var rent = new Rent();
            return View(rent);

            
        }

        //POST METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentOut(int id, [Bind(include: "StartRent,EndRent")] Rent rent)
        {
            if (!BoardExists(id))
            {
                return NotFound();
            }

            rent.BoardId = id;

            if (User.Identity.IsAuthenticated)
            {
                var _userManager = new UserStore<ApplicationUser>(_identityContext);
                var currentUser = _userManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult();

                if (_context.ApplicationUser.Find(currentUser.Id) == null)
                    _context.Add(currentUser);

                rent.ApplicationUserId = currentUser.Id;
            }

            if (rent.StartRent.AddMinutes(5) < DateTime.Now)
            {
                ModelState.AddModelError("StartRent", "Start date must not be sooner than now");
            }
            if (rent.EndRent > rent.StartRent.AddDays(30))
            {
                ModelState.AddModelError("StartRent", "You can only rent boards for 30 days maximum");
            }

            if (rent.StartRent > rent.EndRent)
            {
                ModelState.AddModelError("StartRent", "Start date must be before end date");
            }
            
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
            
            return View(rent);
        }

        private bool BoardExists(int id)
        {
          return _context.Board.Any(e => e.Id == id);
        }
    }
}
