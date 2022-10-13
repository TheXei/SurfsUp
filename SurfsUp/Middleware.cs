using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SurfsUp.Data;
using System.Threading.Tasks;

namespace SurfsUp
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            string path = httpContext.Request.Path;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.Contains("/BoardsUser/GuestRentOut/"))
                {
                    using (var scope = httpContext.RequestServices.CreateScope())
                    {
                        
                        var services = scope.ServiceProvider;

                        try
                        {
                            var _context = services.GetRequiredService<SurfsUpContext>();
                            var ClientIPAddr = httpContext.Connection.RemoteIpAddress?.ToString();
                            if (_context.Guest.Any(x => x.IPAddress == ClientIPAddr.ToString()))
                            {
                                var guest = _context.Guest.FirstOrDefault(x => x.IPAddress == ClientIPAddr.ToString());
                                
                                var rents = _context.Rent.Where(rent => rent.GuestId == guest.Id && rent.EndRent > DateTime.Now);

                                if (rents.Count() > 1)
                                    return;
                            }

                        }
                        catch (Exception ex)
                        {
                            //WriteColor("fuck me...");
                        }
                    }

                }
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder GuestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}
