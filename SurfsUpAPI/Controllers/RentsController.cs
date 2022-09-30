using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;
using SurfsUp.Data;
using Models;

namespace SurfsUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;

        public RentsController(SurfsUpContext context, SurfsUpIdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentDto>> GetRents(string id)
        {
            if (await _context.ApplicationUser.FindAsync(id) == null)
            {
                return NotFound();
            }
            var rents = _context.Rent.Where(x => x.ApplicationUserId != null && x.ApplicationUserId == id);

            if (rents == null)
                return NotFound();

            List<RentDto> dtoRents = new();
            await rents.ForEachAsync(x => dtoRents.Add(new() { BoardId = x.BoardId, StartRent = x.StartRent, EndRent = x.EndRent }));
            return Ok(dtoRents);
        }


        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}

