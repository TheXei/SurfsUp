using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using SurfsUp.Data;
using SurfsUp.Models;
using System.Net.Http;

namespace SurfsUpAPI.Controllers.V2
{
    //[Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
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
        public async Task<ActionResult<GuestRentDto>> GetGuestRent(string id)
        {
            var rent = await _context.Rent.FindAsync(id);

            if (rent == null)
                return NotFound();

            GuestRentDto rentDto = new GuestRentDto()
            {
                BoardId = rent.BoardId,
                StartRent = rent.StartRent,
                EndRent = rent.EndRent
            };

            var user = await _context.Guest.FindAsync(rent.GuestId);

            if (user != null)
            {
                rentDto.FirstName = user.FirstName;
                rentDto.LastName = user.LastName;
                rentDto.PhoneNumber = user.PhoneNumber;
            }

            return rentDto;
        }

        [HttpPost]
        public async Task<ActionResult<GuestRentDto>> GuestRentOut(GuestRentDto rentDto) //int id, [Bind(include: "StartRent,EndRent")] Rent rent, string? userName
        {
            if (!BoardExists(rentDto.BoardId))
            {
                return NotFound();
            }
            Rent rent = new Rent();
            rent.BoardId = rentDto.BoardId;
            rent.StartRent = rentDto.StartRent;
            rent.EndRent = rentDto.EndRent;

            var guest = new Guest();

            if (_context.Guest.Any(x => x.PhoneNumber == rentDto.PhoneNumber))
            {
                guest = _context.Guest.FirstOrDefault(x => x.PhoneNumber == rentDto.PhoneNumber);
                rent.GuestId = guest.Id;
                
            }
            else
            {
                guest = new Guest();
                guest.FirstName = rentDto.FirstName;
                guest.LastName = rentDto.LastName;
                guest.PhoneNumber = rentDto.PhoneNumber;
                guest.IPAddress = rentDto.IPAddress;
                await _context.Guest.AddAsync(guest);
                await _context.SaveChangesAsync();
                rent.GuestId = guest.Id;
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

                    await _context.AddAsync(rent);
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