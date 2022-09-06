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
        public async Task<IActionResult> Index(string search, object type)
        {


            var boards = from m in _context.Board
                         select m;

            if (!String.IsNullOrEmpty(search))
            {
            }



            return View(await boards.ToListAsync());
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

        private bool BoardExists(int id)
        {
          return _context.Board.Any(e => e.Id == id);
        }
    }
}
