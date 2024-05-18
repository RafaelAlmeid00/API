using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;

namespace Api.Adapters_Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BussinesController(IServiceBussines service, IAntiforgery antiforgery) : ControllerBase
    {
        private readonly IServiceBussines _service = service;
        private readonly IAntiforgery _antiforgery = antiforgery;

        // GET: api/Bussines
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet]
        public async Task<ActionResult> GetBussiness([FromBody] Bussines data)
        {
            IResultadoOperacao<List<Bussines>> result = await _service.Search(data);
            return Result(result);
        }

        // GET: api/Bussines/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet("{cnpj}")]
        public async Task<ActionResult> GetBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.GetOne(data);
                return Result(result);
            }
            return NotFound("Bussines não existe");
        }

        // PUT: api/Bussines/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPut("{cnpj}")]
        public async Task<ActionResult> PutBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.Edit(data);
                return Result(result);
            }
            return NotFound("Bussines não existe");
        }

        // POST: api/Bussines
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost]
        public async Task<ActionResult> PostBussines([FromBody] Bussines data)
        {
            IResultadoOperacao<Bussines> result = await _service.Create(data);
            return Result(result);
        }

        // DELETE: api/Bussines
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpDelete("{cnpj}")]
        public async Task<ActionResult> DeleteBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.Delete(data);
                return Result(result);
            }
            return NotFound("Bussines não existe");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LoginBussines([FromBody] BussinesLogin data)
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
            return result.Data?.Token is not null ? Ok(result.Data.Bussines) : BadRequest(result);
        }
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutBussines()
        {
            ILink link = new Link { Rel = "logout_Bussines", Href = "/Bussines/Logout", Method = "POST" };
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
        [HttpPatch("Enable/{cnpj}")]
        public async Task<ActionResult> EnableBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.Enable(data);
                return Result(result);
            }
            return NotFound("Bussines não existe");
        }

        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("Disable/{cnpj}")]
        public async Task<ActionResult> DisableBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.Disable(data);
                return Result(result);
            }
            return NotFound("Bussines não existe");
        }

        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("AlterType/{cnpj}")]
        public async Task<ActionResult> AlterTypeBussines(string cnpj, [FromBody] Bussines data)
        {
            if (cnpj == data.BussCnpj)
            {
                IResultadoOperacao<Bussines> result = await _service.AlterType(data);
                return Result(result);
            }
            return NotFound("Bussines não existe");
        }

        private OkObjectResult Result(dynamic result)
        {
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
