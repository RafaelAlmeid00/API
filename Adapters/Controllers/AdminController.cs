
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
        public async Task<ActionResult> GetAdmins([FromBody] Admin admin)
        {
            IResultadoOperacao<List<Admin>> result = await _service.Search(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        // GET: api/Admin/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAdmin(int id, [FromBody] Admin admin)
        {
            if (id == admin.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.GetOne(admin);
                return result.Data is not null ? Ok(result) : BadRequest(result);
            }
            return NotFound("Admin não existe");
        }

        // PUT: api/Admin/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAdmin(int id, [FromBody] Admin admin)
        {
            if (id == admin.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.Edit(admin);
                return result.Data is not null ? Ok(result) : BadRequest(result);
            }
            return NotFound("Admin não existe");
        }

        // POST: api/Admin
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostAdmin([FromBody] Admin admin)
        {
            IResultadoOperacao<Admin> result = await _service.Create(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/Admin
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdmin(int id, [FromBody] Admin admin)
        {
            if (id == admin.AdmId)
            {
                IResultadoOperacao<Admin> result = await _service.Delete(admin);
                return result.Data is not null ? Ok(result) : BadRequest(result);
            }
            return NotFound("Admin não existe");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LoginAdmin([FromBody] AdminLogin admin)
        {
            IResultadoOperacao<string> result = await _service.Login(admin);
            if (result.Data is not null)
            {
                HttpContext.Session.SetString("AuthToken", result.Data);
            }
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutAdmin([FromBody] AdminLogin admin)
        {
            IResultadoOperacao<string> result = _service.Logout(admin);
            if (result.Data is not null)
            {
                HttpContext.Session.SetString("AuthToken", result.Data);
            }
            return Task.FromResult<ActionResult>(result.Data is not null ? Ok(result) : BadRequest(result));
        }
    }
}
