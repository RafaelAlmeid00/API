using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Domain;
using Api.Infrastructure;
using Api.Interface;

namespace Api.Adapters_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IService<Admin> service) : ControllerBase
    {
        private readonly IService<Admin> _service = service;

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult> GetAdmins([FromBody] Admin admin)
        {
            IResultadoOperacao<List<Admin>> result = await _service.Search(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAdmin([FromBody] Admin admin)
        {
            IResultadoOperacao<Admin> result = await _service.GetOne(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        // PUT: api/Admin/5
        [HttpPut]
        public async Task<ActionResult> PutAdmin([FromBody] Admin admin)
        {
            IResultadoOperacao<Admin> result = await _service.Edit(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        // POST: api/Admin
        [HttpPost]
        public async Task<ActionResult> PostAdmin([FromBody] Admin admin)
        {
            IResultadoOperacao<Admin> result = await _service.Create(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/Admin/5
        [HttpDelete]
        public async Task<ActionResult> DeleteAdmin([FromBody] Admin admin)
        {
            IResultadoOperacao<Admin> result = await _service.Delete(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAdmin([FromBody] Admin admin)
        {
            IResultadoOperacao<string> result = await _service.Login(admin);
            if (result.Data is not null)
            {
                HttpContext.Session.SetString("AuthToken", result.Data);
            }
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> LogoutAdmin([FromBody] Admin admin)
        {
            IResultadoOperacao<string> result = await _service.Logout(admin);
            return result.Data is not null ? Ok(result) : BadRequest(result);
        }
    }
}
