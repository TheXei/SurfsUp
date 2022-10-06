using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Models;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;

        public RentController(SurfsUpContext context, SurfsUpIdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentDto>> GetRent(string id)
        {
            var rent = await _context.Rent.FindAsync(id);

            if (rent == null)
                return NotFound();

            RentDto rentDto = new RentDto()
            {
                BoardId = rent.BoardId,
                StartRent = rent.StartRent,
                EndRent = rent.EndRent
            };

            var user = await _identityContext.Users.FindAsync(rent.ApplicationUserId);

            if (user != null)
            {
                rentDto.UserName = user.UserName;
            }

            return rentDto;
        }

        [HttpPost]
        public async Task<ActionResult<RentDto>> RentOut(RentDto rentDto) //int id, [Bind(include: "StartRent,EndRent")] Rent rent, string? userName
        {
            if (!BoardExists(rentDto.BoardId))
            {
                return NotFound();
            }
            Rent rent = new Rent();
            rent.BoardId = rentDto.BoardId;
            rent.StartRent = rentDto.StartRent;
            rent.EndRent = rentDto.EndRent;

            if (!String.IsNullOrEmpty(rentDto.UserName))
            {
                var _userManager = new UserStore<ApplicationUser>(_identityContext);
                var currentUser = _userManager.FindByNameAsync(rentDto.UserName).GetAwaiter().GetResult();

                if (_context.ApplicationUser.Find(currentUser.Id) == null)
                    _context.Add(currentUser);

                rent.ApplicationUserId = currentUser.Id;
            }

            if (rent.StartRent.AddMinutes(5) < DateTime.Now)
            {
                return BadRequest("Start date must not be sooner than now");
            }
            if (rent.EndRent > rent.StartRent.AddDays(30))
            {
                return BadRequest("You can only rent boards for 30 days maximum");
            }

            if (rent.StartRent > rent.EndRent)
            {
                return BadRequest("Start date must be before end date");
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
            }
            return rentDto;
        }
        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}
