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
        [HttpGet]
        public async Task<ActionResult> GetUsers([FromBody] User data)
        {
            IResultadoOperacao<List<User>> result = await _service.Search(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // GET: api/User/5
        [Authorize]
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
        [ValidateAntiForgeryToken]
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
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.Create(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/User
        [Authorize]
        [ValidateAntiForgeryToken]
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
        public async Task<ActionResult> LoginUser([FromBody] IUserLoginDTO data)
        {
            IResultadoOperacao<dynamic> result = await _service.Login(data);
            if (result.Sucesso)
            {
                string? tokenCsrf = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
                Response.Headers["X-CSRF-TOKEN"] = tokenCsrf;
                HttpContext.Response.Headers.Authorization = "Bearer " + result.Data?.Token;
                HttpContext.Session.SetString("AuthToken", result.Data.Token);
            }
            return result.Data?.Token is not null ? Ok(result.Data.User) : BadRequest(result);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutUser([FromBody] IUserLoginDTO data)
        {
            ILink link = new Link { Rel = "logout_user", Href = "/User/Logout", Method = "POST" };
            string? token = HttpContext.Session.GetString("AuthToken");
            return Task.FromResult<ActionResult>(
                token is not null ? Ok(
                new ResultadoOperacao<string>
                { Sucesso = true, Link = link })
                : BadRequest(
                new ResultadoOperacao<string>
                { Sucesso = false, Erro = "Token não encontrado", Link = link }));
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Enable")]
        public async Task<ActionResult> EnableUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.Enable(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Disable")]
        public async Task<ActionResult> DisableUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.Disable(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("AlterType")]
        public async Task<ActionResult> AlterTypeUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.AlterType(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
