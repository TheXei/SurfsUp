using Microsoft.AspNetCore.Http;
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
    public class BoardController : ControllerBase
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;

        public BoardController(SurfsUpContext context, SurfsUpIdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        [HttpPost("")]
        public async Task<ActionResult<RentDto>> Create([Bind("Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return Ok(board);
            }
            return BadRequest();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<RentDto>> Edit(int id, [Bind("Id, Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] Board board)
        {
            if (id != board.Id || !BoardExists(id))
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
        public async Task<ActionResult<RentDto>> Delete(int id)
        {
            if(!BoardExists(id))
            {
                return NotFound();
            }

            Board board = _context.Board.Find(id);
            _context.Board.Remove(board);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}
