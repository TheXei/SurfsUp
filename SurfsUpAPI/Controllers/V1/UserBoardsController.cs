using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUpAPI.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserBoardsController : ControllerBase
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;

        public UserBoardsController(SurfsUpContext context, SurfsUpIdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        [HttpGet("{UserName}")]
        public async Task<ActionResult<List<Board>>> Index(string? type, string? search, bool? withRent, string UserName)
        {
            var _userManager = new UserStore<ApplicationUser>(_identityContext);
            var currentUser = await _userManager.FindByNameAsync(UserName);

            if (currentUser == null)
                return NotFound();

            var boards = _context.UserBoard.Where(b => b.OwnerId == currentUser.Id);

            boards = boards.Include(r => r.Rent);

            await boards.Where(board => board.Rent != null && board.Rent.EndRent < DateTime.Now).ForEachAsync(board => board.Rent = null);
            {
                await _context.SaveChangesAsync();
            }
            //if (withRent != true)
            //{
            //    boards = boards.Where(board => board.Rent == null);
            //}

            if (!String.IsNullOrEmpty(search))
                boards = from b in boards where b.Name.ToLower()!.Contains(search.ToLower()) select b;

            if (!String.IsNullOrEmpty(type))
            {
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
            };
            return new List<Board>(boards);
        }
    }
}
