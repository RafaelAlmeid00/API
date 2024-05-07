using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;

namespace Api.Adapters_Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IServiceUser<User> service, IAntiforgery antiforgery) : ControllerBase
    {
        private readonly IServiceUser<User> _service = service;
        private readonly IAntiforgery _antiforgery = antiforgery;

        // GET: api/User
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet]
        public async Task<ActionResult> GetUsers([FromBody] User data)
        {
            IResultadoOperacao<List<User>> result = await _service.Search(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
        
        // GET: api/User/GeneratePDF
        // [Authorize]
        // [RequireAntiforgeryToken]
        // [HttpGet("GeneratePDF")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> GeneratePDF([FromBody] User data)
        // {
        //     var userData = await _service.GetUserPDFData(data);
        //     var pdfBytes = GeneratePDFUsingReportBuilder(userData);
        //     return File(pdfBytes);
        // }


        // GET: api/User/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet("{cpf}")]
        public async Task<ActionResult> GetUser(string cpf, [FromBody] User data)
        {
            if (cpf == data.UserCpf)
            {
                IResultadoOperacao<User> result = await _service.GetOne(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("User não existe");
        }

        // PUT: api/User/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPut("{cpf}")]
        public async Task<ActionResult> PutUser(string cpf, [FromBody] User data)
        {
            if (cpf == data.UserCpf)
            {
                IResultadoOperacao<User> result = await _service.Edit(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("User não existe");
        }

        // POST: api/User
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.Create(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/User
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpDelete("{cpf}")]
        public async Task<ActionResult> DeleteUser(string cpf, [FromBody] User data)
        {
            if (cpf == data.UserCpf)
            {
                IResultadoOperacao<User> result = await _service.Delete(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("User não existe");
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginUser([FromBody] UserLogin data)
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
            return result.Data?.Token is not null ? Ok(result.Data.User) : BadRequest(result);
        }
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutUser()
        {
            ILink link = new Link { Rel = "logout_user", Href = "/User/Logout", Method = "POST" };
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

        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("Enable/{cpf}")]
        public async Task<ActionResult> EnableUser(string cpf, [FromBody] User data)
        {
            if (cpf == data.UserCpf)
            {
                IResultadoOperacao<User> result = await _service.Enable(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("User não existe");
        }

        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("Disable/{cpf}")]
        public async Task<ActionResult> DisableUser(string cpf, [FromBody] User data)
        {
            if (cpf == data.UserCpf)
            {
                IResultadoOperacao<User> result = await _service.Disable(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("User não existe");
        }

        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("AlterType/{cpf}")]
        public async Task<ActionResult> AlterTypeUser(string cpf, [FromBody] User data)
        {
            if (cpf == data.UserCpf)
            {
                IResultadoOperacao<User> result = await _service.AlterType(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("User não existe");
        }
    }
}
