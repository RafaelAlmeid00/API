using Api.Interface;

namespace Api.Services
{
    public partial class Auth : IAuth
    {
        public string CreateTokenCSRF()
        {
            string? token = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
            return 
            
                    HttpContext? httpContext = _httpContextAccessor.HttpContext;
                    string? tokenCsrf = httpContext?.Request.Cookies[".AspNetCore.Session"];
                    if (tokenCsrf != null)
                    {
                        httpContext?.Response.Cookies.Append("X-CSRF-TOKEN", tokenCsrf, new CookieOptions
                        {
                            SameSite = SameSiteMode.None,
                            HttpOnly = false,
                            Secure = true
                        });
                    }
        }
    }
}