namespace Ecom.ApI.Middleware
{
    public class ReadJwtTokenFromCoockies
    {
        private readonly RequestDelegate _next;
        public ReadJwtTokenFromCoockies(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
          
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                context.Request.Headers["Authorization"] = "Bearer " + token;
            }
               await _next(context);

        }





    }
}
