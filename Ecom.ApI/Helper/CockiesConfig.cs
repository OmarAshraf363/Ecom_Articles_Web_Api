namespace Ecom.ApI.Helper
{
    public static class CockiesConfig
    {
        public static void SetCookie(HttpContext context, string key, string value)
        {
            var isHttps = context.Request.IsHttps;

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = isHttps,
                SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            };

            context.Response.Cookies.Append(key, value, options);
        }
        public static void RemoveCookie(HttpContext context, string key)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = context.Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            };
            context.Response.Cookies.Delete(key, options);


        }
        public static string GetCookie(HttpContext context, string key)
        {
            if (context.Request.Cookies[key]!=null)
            {

           return  context.Request.Cookies[key];
            }
            return null;
        }
            
           

    }
}
