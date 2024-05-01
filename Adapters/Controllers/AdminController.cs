using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Api.Adapters_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IServiceAdmin<Admin> service) : ControllerBase
    {
        private readonly IServiceAdmin<Admin> _service = service;

        // GET: api/Admin
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAdmins([FromBody] Admin data)
        {
            IResultadoOperacao<List<Admin>> result = await _service.Search(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // GET: api/Admin/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAdmin(int id, [FromBody] Admin data)
        {
            if (id == data.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.GetOne(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("Admin não existe");
        }

        // PUT: api/Admin/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAdmin(int id, [FromBody] Admin data)
        {
            if (id == data.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.Edit(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("Admin não existe");
        }

        // POST: api/Admin
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostAdmin([FromBody] Admin data)
        {
            IResultadoOperacao<Admin> result = await _service.Create(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/Admin
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdmin(int id, [FromBody] Admin data)
        {
            if (id == data.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.Delete(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("Admin não existe");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LoginAdmin([FromBody] AdminLogin data)
        {
            IResultadoOperacao<dynamic> result = await _service.Login(data);
            if (result.Sucesso)
            {
                HttpContext.Response.Headers.Append("Authorization", result.Data.Token);
                HttpContext.Session.SetString("AuthToken", result.Data.Token);
            }
            return result.Data?.Token is not null ? Ok(result.Data.Admin) : BadRequest(result);
        }
        [Authorize]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutAdmin([FromBody] AdminLogin data)
        {
            IResultadoOperacao<string> result = _service.Logout(data);
            return Task.FromResult<ActionResult>(result.Sucesso ? Ok(result) : BadRequest(result));
        }
    }
}
