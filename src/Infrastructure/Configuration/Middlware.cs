using System.IdentityModel.Tokens.Jwt;
using System.Text;
public class AuthorizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) &&
            context.Request.Headers.TryGetValue("X-CSRF-TOKEN", out var CSRFHeader))
            {
                string token = authorizationHeader.ToString().Replace("Bearer ", "");
                string tokenCsrf = CSRFHeader.ToString();
                JwtSecurityTokenHandler handler = new();

                JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                string? levelClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "Level")?.Value;
                string? AdminClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "AdminId")?.Value;
                string? UserClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "UserCPF")?.Value;
                string? BussinesClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "BussinesCNPJ")?.Value;
                string? tokenCsrfValidation = context.Session.GetString("X-CSRF-TOKEN");

                Console.WriteLine(" levelClaim " + levelClaim);
                Console.WriteLine(" AdminClaim " + AdminClaim);
                Console.WriteLine(" UserClaim " + UserClaim);
                Console.WriteLine(" BussinesClaim " + BussinesClaim);
                Console.WriteLine(" jsonToken " + jsonToken);
                Console.WriteLine(" tokenCsrf " + tokenCsrf);
                Console.WriteLine(" tokenCsrfValidation " + tokenCsrfValidation);
                Console.WriteLine(" Valdiation " + tokenCsrf != tokenCsrfValidation);

                if (tokenCsrf != tokenCsrfValidation)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await WriteUnauthorizedResponse(context);
                    Console.WriteLine("parada mortal");
                    return;
                }

                if (context.Request.Path.StartsWithSegments("/api/ListCpf") && BussinesClaim is null && AdminClaim is null && context.Request.Method != "GET")
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await WriteUnauthorizedResponse(context);
                    Console.WriteLine("parada 0");
                    return;
                }

                Console.WriteLine("parada 1");
                if (context.Request.Path.StartsWithSegments("/api/Bussines")
                    && (context.Request.Path != "/api/Bussines/Login"
                    || context.Request.Path != "/api/Bussines/Logout")
                    || context.Request.Path.StartsWithSegments("/api/User")
                    && context.Request.Path != "/api/User/Login"
                    && context.Request.Path != "/api/User/Logout")
                {
                    if (AdminClaim is null && UserClaim is null && BussinesClaim is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await WriteUnauthorizedResponse(context);
                        Console.WriteLine("parada 2");
                        return;
                    }
                }

                Console.WriteLine("parada 3");
                if (context.Request.Path.StartsWithSegments("/api/Admin")
                    && context.Request.Path != "/api/Admin/Login"
                    && context.Request.Path != "/api/Admin/Logout")
                {
                    if (AdminClaim is null || levelClaim == "1"
                        || (levelClaim == "2" && context.Request.Method != "GET"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await WriteUnauthorizedResponse(context);
                        Console.WriteLine("parada 4");
                        return;
                    }
                }
                Console.WriteLine("parada 5");
                await _next(context);
                return;
            }
            if (context.Request.Path == "/api/Admin/Login" || context.Request.Path == "/api/User/Login" || context.Request.Path == "/api/Bussines/Login")
            {
                Console.WriteLine("parada 6");
                await _next(context);
                return;
            }
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await WriteUnauthorizedResponse(context);
            Console.WriteLine("parada 7");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler o token: {ex.Message}");
            await WriteUnauthorizedResponse(context);
            Console.WriteLine("parada 8");
            return;
        }

    }

    private static async Task WriteUnauthorizedResponse(HttpContext context)
    {
        string responseMessage = "Não autorizado";
        byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
        await context.Response.Body.WriteAsync(responseBytes);
    }
}
