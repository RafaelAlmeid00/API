using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;

namespace Api.Adapters_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListCpfController(IServiceListCpf service, IAntiforgery antiforgery) : ControllerBase
    {
        private readonly IServiceListCpf _service = service;
        private readonly IAntiforgery _antiforgery = antiforgery;

        // GET: api/ListCpf
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet]
        public async Task<ActionResult> GetListCpfs([FromBody] ListCpf data)
        {
            IResultadoOperacao<List<ListCpf>> result = await _service.Search(data);
            return Result(result);
        }

        // GET: api/ListCpf/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet("{cpf}")]
        public async Task<ActionResult> GetListCpf(string cpf, [FromBody] ListCpf data)
        {
            if (cpf == data.ListCPF)
            {
                IResultadoOperacao<ListCpf> result = await _service.GetOne(data);
                return Result(result);
            }
            return NotFound("ListCpf não existe");
        }

        // PUT: api/ListCpf/5
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPut("{cpf}")]
        public async Task<ActionResult> PutListCpf(string cpf, [FromBody] ListCpf data)
        {
            if (cpf == data.ListCPF)
            {
                IResultadoOperacao<ListCpf> result = await _service.Edit(data);
                return Result(result);
            }
            return NotFound("ListCpf não existe");
        }

        // POST: api/ListCpf
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost]
        public async Task<ActionResult> PostListCpf([FromBody] ListCpf data)
        {
            IResultadoOperacao<ListCpf> result = await _service.Create(data);
            return Result(result);
        }

        // POST: api/ListCpf
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost("Excel")]
        public async Task<ActionResult> PostListCpfExcel([FromForm] IFormFile data)
        {
            IResultadoOperacao<dynamic> result = await _service.CreateWithExcel(data);
            return Result(result);
        }

        // DELETE: api/ListCpf
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpDelete("{cpf}")]
        public async Task<ActionResult> DeleteListCpf(string cpf, [FromBody] ListCpf data)
        {
            if (cpf == data.ListCPF)
            {
                IResultadoOperacao<ListCpf> result = await _service.Delete(data);
                return Result(result);
            }
            return NotFound("ListCpf não existe");
        }
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("AlterType/{cpf}")]
        public async Task<ActionResult> AlterTypeListCpf(string cpf, [FromBody] ListCpf data)
        {
            if (cpf == data.ListCPF)
            {
                IResultadoOperacao<ListCpf> result = await _service.AlterType(data);
                return Result(result);
            }
            return NotFound("ListCpf não existe");
        }
        private OkObjectResult Result(dynamic result)
        {
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
