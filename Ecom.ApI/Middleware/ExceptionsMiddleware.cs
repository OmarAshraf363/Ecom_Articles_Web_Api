using Ecom.ApI.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.ApI.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheTimeSpan = TimeSpan.FromSeconds(90);

        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment env, IMemoryCache cache)
        {
            _next = next;
            _env = env;
            _cache = cache;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                //Check if request is allowed such as too many requests or not
                if (!IsRequestAllowed(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ApiException((int)HttpStatusCode.TooManyRequests, "Too many requests");
                    var json = JsonSerializer.Serialize(response);
                    await context.Response.WriteAsync(json);
                    return;
                }
                //All is done complete with next step
                await _next(context);
            }
            catch (Exception ex)
            {
                //If any exception occurs then handle it
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _env.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    :
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }


        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cachket = $"RequestAllowed-{ip}";
            var dateNow = DateTime.Now;
            var (timesTamp, count) = _cache.GetOrCreate(cachket, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;
                return (dateNow, 0);
            });
            if (dateNow - timesTamp > _cacheTimeSpan)
            {
                if (count > 80)
                {
                    return false;
                }
                _cache.Set(cachket, (timesTamp, count + 1), _cacheTimeSpan);
            }
            else
            {
                _cache.Set(cachket, (timesTamp, count), _cacheTimeSpan);

            }
            return true;
        }


        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["X-Xss-Protection"] = "1; mode=block";
            context.Response.Headers["Referrer-Policy"] = "no-referrer";



        }
    } 
}
