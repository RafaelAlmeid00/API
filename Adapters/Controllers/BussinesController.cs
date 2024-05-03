using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;
using Newtonsoft.Json;

namespace Api.Adapters_Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BussinesController(IServiceBussines<Bussines> service, IAntiforgery antiforgery) : ControllerBase
    {
        private readonly IServiceBussines<Bussines> _service = service;
        private readonly IAntiforgery _antiforgery = antiforgery;

        // GET: api/Bussines
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetBussiness([FromBody] Bussines data)
        {
            IResultadoOperacao<List<Bussines>> result = await _service.Search(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // GET: api/Bussines/5
        [Authorize]
        [HttpGet("{cnpj}")]
        public async Task<ActionResult> GetBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.GetOne(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("Bussines não existe");
        }

        // PUT: api/Bussines/5
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPut("{cnpj}")]
        public async Task<ActionResult> PutBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.Edit(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("Bussines não existe");
        }

        // POST: api/Bussines
        [HttpPost]
        public async Task<ActionResult> PostBussines([FromBody] Bussines data)
        {
            IResultadoOperacao<Bussines> result = await _service.Create(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/Bussines
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpDelete("{cnpj}")]
        public async Task<ActionResult> DeleteBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.Delete(data);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            return NotFound("Bussines não existe");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LoginBussines([FromBody] IBussinesLoginDTO data)
        {
            IResultadoOperacao<dynamic> result = await _service.Login(data);
            if (result.Sucesso)
            {
                string? tokenCsrf = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
                Response.Headers["X-CSRF-TOKEN"] = tokenCsrf;
                HttpContext.Response.Headers.Authorization = "Bearer " + result.Data?.Token;
                string token = result.Data.Token;
                HttpContext.Session.SetString("AuthToken", token);
            }
            return result.Data?.Token is not null ? Ok(result.Data.Bussines) : BadRequest(result);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutBussines([FromBody] IBussinesLoginDTO data)
        {
            ILink link = new Link { Rel = "logout_Bussines", Href = "/Bussines/Logout", Method = "POST" };
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
        public async Task<ActionResult> EnableBussines([FromBody] Bussines data)
        {
            IResultadoOperacao<Bussines> result = await _service.Enable(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Disable")]
        public async Task<ActionResult> DisableBussines([FromBody] Bussines data)
        {
            IResultadoOperacao<Bussines> result = await _service.Disable(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("AlterType")]
        public async Task<ActionResult> AlterTypeBussines([FromBody] Bussines data)
        {
            IResultadoOperacao<Bussines> result = await _service.AlterType(data);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
