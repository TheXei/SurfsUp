using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SurfsUp.Data;
using SurfsUp.Models;
using SurfsUp.Models.Weather;
using System.Text.Json;

namespace SurfsUpAPI.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly SurfsUpContext _context;
        private readonly SurfsUpIdentityContext _identityContext;
        private readonly HttpClient _httpClient;

        public ForecastController(SurfsUpContext context, SurfsUpIdentityContext identityContext, HttpClient httpClient)
        {
            _context = context;
            _identityContext = identityContext;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
        }
        [HttpGet("")]
        public async Task<Root> Forecast(string search, int days)
        {
            days = Math.Clamp(days, 1, 5);
            days *= 8;
            string request = $"forecast?q={search}&units=metric&cnt={days}&appid=c80a3333c44ae99b759212a5d1903600";
            string forecast = await (await _httpClient.GetAsync(request)).Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<Root>(forecast);
            return response;
            
        }
    }
}
