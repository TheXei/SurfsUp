using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;
using SurfsUp.Data;
using Models;

namespace SurfsUpAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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

        [HttpGet("{UserName}")]
        public async Task<ActionResult<List<RentDto>>> GetRents(string UserName)
        {
            var _userManager = new UserStore<ApplicationUser>(_identityContext);
            var currentUser = _userManager.FindByNameAsync(UserName).GetAwaiter().GetResult();

            var rent = _context.Rent.Where(b => b.ApplicationUserId == currentUser.Id);

            if (rent == null)
                return NotFound();

            List<RentDto> rentList = new List<RentDto>();

            await rent.ForEachAsync(b =>
            {
                rentList.Add(new RentDto
                {
                    BoardId = b.BoardId,
                    StartRent = b.StartRent,
                    EndRent = b.EndRent,
                    UserName = currentUser.UserName
                });
            });

            return rentList;
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }

        private bool UserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

    }
}
