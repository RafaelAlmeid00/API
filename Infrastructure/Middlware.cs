using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class AuthorizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) && context.Request.Headers.TryGetValue("X-CSRF-TOKEN", out var CSRFHeader))
        {
            string token = authorizationHeader.ToString().Replace("Bearer ", "");
            string tokenCsrf = CSRFHeader.ToString();
            JwtSecurityTokenHandler handler = new();
            try
            {
                JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                string? levelClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "Level")?.Value;
                string? AdminClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "AdminId")?.Value;
                string? UserClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "UserCPF")?.Value;

                if (context.Request.Path != "api/Admin/Login" && context.Request.Path != "api/Admin/Logout")
                {
                    if (AdminClaim is not null && levelClaim != "1" && !(levelClaim == "2" && context.Request.Method != "Get"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        string responseMessage = "Não autorizado";
                        byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                        context.Response.Body.Write(responseBytes, 0, responseBytes.Length);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o token JWT: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                string responseMessage = "Não autorizado";
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                context.Response.Body.Write(responseBytes, 0, responseBytes.Length);
                return;
            }
        }
        await _next(context);
    }
}