using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUpAPI.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
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
        [HttpGet("{id}")]
        public async Task<ActionResult<RentDto>> GetBoard(int id)
        {
            var board = await _context.Board.FindAsync(id);

            if (board == null)
                return NotFound();

            return Ok(board);
        }
    }
}
