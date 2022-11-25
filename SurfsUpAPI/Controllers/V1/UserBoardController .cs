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
        public async Task<ActionResult<Board>> Create([Bind("Name,Length,Width,Thickness,Volume,Type,Price,Equipments,ImageURL")] UserBoard board, string UserName)
        {
            var _userManager = new UserStore<ApplicationUser>(_identityContext);
            var currentUser = await _userManager.FindByNameAsync(UserName);

            if (currentUser == null)
                return NotFound();
            
            board.OwnerId = currentUser.Id;

            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return Ok(board);
            }
            return BadRequest();
        }
    }
}
