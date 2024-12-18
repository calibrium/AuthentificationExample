namespace AuthentificationExample.Server.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetCookie(this HttpContext context, string key, string value, CookieOptions options)
        {
            if (context?.Response?.Cookies != null)
            {
                context.Response.Cookies.Append(key, value, options);
            }
        }
    }
}
