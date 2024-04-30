using System.IdentityModel.Tokens.Jwt;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            var token = authorizationHeader.ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                var levelClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "Level")?.Value;
                if (context.Request.Path != "api/Admin/Login" && context.Request.Path != "api/Admin/Logout")
                {
                    if (levelClaim != "1" && !(levelClaim == "2" && context.Request.Method != "Get"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o token JWT: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
        await _next(context);
    }
}