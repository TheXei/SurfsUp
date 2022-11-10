using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace SurfsUp.Utility
{
    public interface IRateLimiter<T>
    {
        bool ShouldBlock(T entity);
        int RequestLimit { get; set; }
        TimeSpan LimitPeriod { get; set; }
    }

    public class IPLimiter : IRateLimiter<IPAddress>
    {
        private Dictionary<IPAddress, List<DateTime>> _loggedRequests;
        //private readonly ILogger _logger;
        private RequestDelegate _next;
        public int RequestLimit { get; set; } = 5;
        public TimeSpan LimitPeriod { get; set; } = new(0, minutes: 3, 0);

        public IPLimiter(RequestDelegate next)
        {
            _next = next;
            _loggedRequests = new();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;
            if (path.ToLower().StartsWith("/BoardsUser/GuestRentOut/".ToLower()))
            {
                IPAddress clientAddress = context.Connection.RemoteIpAddress;
                DateTime timeStamp = DateTime.Now;
                LogRequest(clientAddress, timeStamp);
                if (ShouldBlock(clientAddress))
                {
                    Block(context);
                    return;
                }
            }

            await _next.Invoke(context);
        }

        public bool ShouldBlock(IPAddress entity)
        {
            if (entity == null)
                throw new NullReferenceException();

            if (_loggedRequests[entity].Count(dt => dt >= DateTime.Now - LimitPeriod) >= RequestLimit)
                return true;
            return false;
        }

        private void Block(HttpContext context)
        {
            context.Response.WriteAsync("BLOCKED");
        }

        private void LogRequest(IPAddress client, DateTime timeStamp)
        {
            if (client == null)
                throw new NullReferenceException();

            if (_loggedRequests.ContainsKey(client))
                _loggedRequests[client].Add(timeStamp);
            else
                _loggedRequests.Add(client, new() { timeStamp });
        }
    }

    public static class IPLimiterExtensions
    {
        public static IApplicationBuilder UseIPRateLimiting(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IPLimiter>();
        }
    }
}
