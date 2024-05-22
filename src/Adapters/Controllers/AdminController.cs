using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Domain.Results;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;

namespace Api.Adapters_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IServiceAdmin service, IAntiforgery antiforgery) : ControllerBase
    {
        private readonly IServiceAdmin _service = service;
        private readonly IAntiforgery _antiforgery = antiforgery;

        // GET: api/Admin
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet]
        public async Task<ActionResult> GetAdmins([FromBody] Admin data)
        {
            IResultadoOperacao<List<Admin>> result = await _service.Search(data);
            return Result(result);
        }

        // GET: api/Admin/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAdmin(int id, [FromBody] Admin data)
        {
            if (id == data.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.GetOne(data);
                return Result(result);
            }
            return NotFound("Admin não existe");
        }

        // PUT: api/Admin/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAdmin(int id, [FromBody] Admin data)
        {
            if (id == data.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.Edit(data);
                return Result(result);
            }
            return NotFound("Admin não existe");
        }

        // POST: api/Admin
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost]
        public async Task<ActionResult> PostAdmin([FromBody] Admin data)
        {
            IResultadoOperacao<Admin> result = await _service.Create(data);
            return Result(result);
        }

        // DELETE: api/Admin
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdmin(int id, [FromBody] Admin data)
        {
            if (id == data.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.Delete(data);
                return Result(result);
            }
            return NotFound("Admin não existe");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LoginAdmin([FromBody] AdminLogin data)
        {
            IResultadoOperacao<dynamic> result = await _service.Login(data);
            if (result.Sucesso)
            {
                string? tokenCsrf = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
                Response.Headers["X-CSRF-TOKEN"] = tokenCsrf;
                HttpContext.Response.Headers.Authorization = "Bearer " + result.Data?.Token;
                string token = result.Data.Token;
                HttpContext.Session.SetString("AuthToken", token);
                HttpContext.Session.SetString("X-CSRF-TOKEN", tokenCsrf);
            }
            return result.Data?.Token is not null ? Ok(result.Data.Admin) : BadRequest(result);
        }
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutAdmin()
        {
            ILink link = new Link { Rel = "logout_Admin", Href = "/Admin/Logout", Method = "POST" };
            string? token = HttpContext.Session.GetString("AuthToken");
            string? tokenCsrf = HttpContext.Session.GetString("X-CSRF-TOKEN");
            return Task.FromResult<ActionResult>(
                token is not null &&
                tokenCsrf is not null ? Ok(
                new ResultadoOperacao<string>
                { Sucesso = true, Link = link })
                : BadRequest(
                new ResultadoOperacao<string>
                { Sucesso = false, Erro = "Token não encontrado", Link = link }));
        }

        private OkObjectResult Result(dynamic result)
        {
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
