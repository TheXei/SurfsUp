using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text.Json;
using Models;
using System.Text;
using System.Net.Http.Json;
using Models.DTOs;
using Newtonsoft.Json;


namespace SurfsUp.Controllers
{
    public class BoardsUserController : Controller
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;
        private readonly HttpClient _httpClient;

        public BoardsUserController(SurfsUpContext context, SurfsUpIdentityContext identityContext, HttpClient httpClient)
        {
            _context = context;
            _identityContext = identityContext;
            //_httpClient = new HttpClient()
            //{
            //    BaseAddress = new Uri("https://localhost:7277/api/")
            //};
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7277/api/");
            Console.WriteLine("Base address is" + httpClient.BaseAddress);
        }

        async Task<string> GetAsync(string call)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(call);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        //public Task SortAndSearch(IQueryable<Board> boardList, string type, string search)
        //{
        //    //IQueryable<Board> boards = boardList;


        //    return Task.CompletedTask;
        //}

        // GET: Boards
        public async Task<IActionResult> Index(string currentFilter,
                                                string search,
                                                int? pageNumber,
                                                string type,
                                                bool rentError)
        {
            ViewData["CurrentFilter"] = search;
            ViewData["Type"] = type;

            if (rentError)
                ViewData["Locked"] = "Someone else is currently renting this Board. Please try again later.";

            if (search != null)
            {
                pageNumber = 1;
            }
            else
            {
                search = currentFilter;
            }
            string request = $"v1/boards/?search={search}&type={type}";

            if (!User.Identity.IsAuthenticated)
            {
                request = $"v2/boards/?search={search}&type={type}";
            }
            bool premium = User.Identity.IsAuthenticated;

            var jsonString = GetAsync(request).Result;
            var boards = JsonConvert.DeserializeObject<List<Board>>(jsonString);

            int pageSize = 8;
            return View(await PaginatedList<Board>.CreateAsync(boards, pageNumber ?? 1, pageSize));
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

        //method that compares to long values
        

        //GET MOETHOD
        public async Task<IActionResult> RentOut(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            //var boardRequest = await _httpClient.GetFromJsonAsync("Rents", id);
            //var board = await JsonSerializer.DeserializeAsync<Board>(boardRequest);

            var board = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);

            if (board == null)
            {
                return NotFound();
            }
            
            TimeSpan isTimerOver = DateTime.Now - board.LockDate;
            if (isTimerOver.TotalMinutes < 5)
            {
                return RedirectToAction(nameof(Index), new { @rentError = true });
            }
            board.LockDate = DateTime.Now;
            await _context.SaveChangesAsync();
            var rent = new Rent();
            return View(rent);
        }

        //POST METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentOut(int id, [Bind(include: "StartRent,EndRent")] Rent rent)
        {
            //instansiere ny RentDto
            RentDto rentDto = new RentDto()
            {
                BoardId = id,
                StartRent = rent.StartRent,
                EndRent = rent.EndRent
            };

            //tilføjer bruger hvis logger ind
            if (User.Identity.IsAuthenticated)
            {
                rentDto.UserName = User.Identity.Name;
            }

            if (rent.StartRent.AddMinutes(5) < DateTime.Now)
            {
                ModelState.AddModelError("StartRent", "Start date must not be sooner than now");
            }
            if (rent.EndRent > rent.StartRent.AddDays(30))
            {
                ModelState.AddModelError("StartRent", "You can only rent boards for 30 days maximum");
            }

            if (rent.StartRent > rent.EndRent)
            {
                ModelState.AddModelError("StartRent", "Start date must be before end date");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    //post as json.
                    var jsonPost = await _httpClient.PostAsJsonAsync("v1.0/rent", rentDto);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(rentDto);
        }
        public async Task<IActionResult> GuestRentOut(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);

            if (board == null)
            {
                return NotFound();
            }

            TimeSpan isTimerOver = DateTime.Now - board.LockDate;
            if (isTimerOver.TotalMinutes < 5)
            {
                return RedirectToAction(nameof(Index), new { @rentError = true });
            }
            board.LockDate = DateTime.Now;
            await _context.SaveChangesAsync();
            var rent = new GuestRentViewModel
            {
                Rent = new Rent(),
                Guest = new Guest()
            };
            return View(rent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuestRentOut(int id, [Bind(include: "Rent, Guest")] GuestRentViewModel rentVM)
        {
            //instansiere ny RentDto
            

            if (rentVM.Rent.StartRent.AddMinutes(5) < DateTime.Now)
            {
                ModelState.AddModelError("StartRent", "Start date must not be sooner than now");
            }
            if (rentVM.Rent.EndRent > rentVM.Rent.StartRent.AddDays(30))
            {
                ModelState.AddModelError("StartRent", "You can only rent boards for 30 days maximum");
            }

            if (rentVM.Rent.StartRent > rentVM.Rent.EndRent)
            {
                ModelState.AddModelError("StartRent", "Start date must be before end date");
            }
            GuestRentDto rentDto = new GuestRentDto()
            {
                BoardId = id,
                StartRent = rentVM.Rent.StartRent,
                EndRent = rentVM.Rent.EndRent,
                FirstName = rentVM.Guest.FirstName,
                LastName = rentVM.Guest.LastName,
                PhoneNumber = rentVM.Guest.PhoneNumber,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            if (ModelState.IsValid)
            {
                try
                {
                    
                    //post as json.
                    var jsonPost = await _httpClient.PostAsJsonAsync("v2/rent", rentDto);

                }
                catch (Exception ex)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(rentVM);
        }

        private bool BoardExists(int id)
        {
          return _context.Board.Any(e => e.Id == id);
        }
    }
}
