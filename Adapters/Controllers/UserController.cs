using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;


namespace Api.Adapters_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IServiceUser<User> service) : ControllerBase
    {
        private readonly IServiceUser<User> _service = service;

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
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.Create(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/User
        [Authorize]
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
                HttpContext.Response.Headers.Append("Authorization", result.Data?.Token);
                HttpContext.Session.SetString("AuthToken", result.Data.Token);
            }
            return result.Data?.Token is not null ? Ok(result.Data.User) : BadRequest(result);
        }
        [Authorize]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutUser([FromBody] IUserLoginDTO data)
        {
            IResultadoOperacao<string> result = _service.Logout(data);
            return Task.FromResult<ActionResult>(result.Sucesso ? Ok(result) : BadRequest(result));
        }

        [Authorize]
        [HttpPost("Enable")]
        public async Task<ActionResult> EnableUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.Enable(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpPost("Disable")]
        public async Task<ActionResult> DisableUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.Disable(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpPost("AlterType")]
        public async Task<ActionResult> AlterTypeUser([FromBody] User data)
        {
            IResultadoOperacao<User> result = await _service.AlterType(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
