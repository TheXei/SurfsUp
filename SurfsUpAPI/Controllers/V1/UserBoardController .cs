using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUpAPI.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserBoardController : ControllerBase
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;

        public UserBoardController(SurfsUpContext context, SurfsUpIdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        [HttpPost("{UserName}")]
        public async Task<ActionResult<Board>> Create([Bind("Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] Board board, string UserName)
        {
            var _userManager = new UserStore<ApplicationUser>(_identityContext);
            var currentUser = await _userManager.FindByNameAsync(UserName);

            if (currentUser == null)
                return NotFound();

            UserBoard ub = new()
            {
                Name = board.Name,
                Length = board.Length,
                Width = board.Width,
                Thickness = board.Thickness,
                Volume = board.Volume,
                Type = board.Type,
                Price = board.Price,
                Equipments = board.Equipments,
                ImageURL = board.ImageURL,
                OwnerId = currentUser.Id,
            };
            if (ModelState.IsValid)
            {
                _context.Add(ub);
                await _context.SaveChangesAsync();
                return Ok(board);
            }
            return BadRequest();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, string UserName, [Bind("Id, Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] Board board)
        {
            var _userManager = new UserStore<ApplicationUser>(_identityContext);
            var currentUser = await _userManager.FindByNameAsync(UserName);

            if (currentUser == null)
                return  NotFound();
            if (id != board.Id || !BoardExists(id, currentUser.Id))
                return BadRequest();

            var boardToUpdate = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);

            if (ModelState.IsValid)
            {
                // LOL
                boardToUpdate.Name = board.Name;
                boardToUpdate.Length = board.Length;
                boardToUpdate.Width = board.Width;
                boardToUpdate.Thickness = board.Thickness;
                boardToUpdate.Volume = board.Volume;
                boardToUpdate.Type = board.Type;
                boardToUpdate.Price = board.Price;
                boardToUpdate.Equipments = board.Equipments;
                boardToUpdate.ImageURL = board.ImageURL;


                await _context.SaveChangesAsync();
                return Ok(board);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, string UserName)
        {
            var _userManager = new UserStore<ApplicationUser>(_identityContext);
            var currentUser = await _userManager.FindByNameAsync(UserName);

            if (currentUser == null)
                return NotFound();
            if (!BoardExists(id, currentUser.Id))
            {
                return NotFound();
            }

            Board board = _context.Board.Find(id);
            _context.Board.Remove(board);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool BoardExists(int boardId, string userId)
        {
            return _context.UserBoard.Any(u => u.Id == boardId && u.OwnerId == userId);
        }
    }
}
