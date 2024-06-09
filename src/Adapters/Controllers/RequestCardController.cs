using Microsoft.AspNetCore.Mvc;
using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Api.Adapters_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestCardController : ControllerBase
    {
        private readonly IServiceRequestCard _service;
        private readonly IAntiforgery _antiforgery;

        public RequestCardController(IServiceRequestCard service, IAntiforgery antiforgery)
        {
            _service = service;
            _antiforgery = antiforgery;
        }

        // GET: api/RequestCard
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet]
        public async Task<ActionResult> GetRequestCards([FromBody] RequestCard data)
        {
            IResultadoOperacao<List<RequestCard>> result = await _service.Search(data);
            return Result(result);
        }

        // GET: api/RequestCard/{id}
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRequestCard(int id, [FromBody] RequestCard data)
        {
            if (id == data.ReqId)
            {
                IResultadoOperacao<RequestCard> result = await _service.GetOne(data);
                return Result(result);
            }
            return NotFound("RequestCard não existe");
        }

        // PUT: api/RequestCard/{id}
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutRequestCard(int id, [FromBody] RequestCard data)
        {
            if (id == data.ReqId)
            {
                IResultadoOperacao<RequestCard> result = await _service.Edit(data);
                return Result(result);
            }
            return NotFound("RequestCard não existe");
        }

        // POST: api/RequestCard
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost]
        public async Task<ActionResult> PostRequestCard([FromBody] RequestCard data)
        {
            IResultadoOperacao<RequestCard> result = await _service.Create(data);
            return Result(result);
        }

        // DELETE: api/RequestCard/{id}
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRequestCard(int id, [FromBody] RequestCard data)
        {
            if (id == data.ReqId)
            {
                IResultadoOperacao<RequestCard> result = await _service.Delete(data);
                return Result(result);
            }
            return NotFound("RequestCard não existe");
        }

        // PATCH: api/RequestCard/AlterType/{id}
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("AlterType/{id}")]
        public async Task<ActionResult> AlterTypeRequestCard(int id, [FromBody] RequestCard data)
        {
            if (id == data.ReqId)
            {
                IResultadoOperacao<RequestCard> result = await _service.AlterType(data);
                return Result(result);
            }
            return NotFound("RequestCard não existe");
        }

        private OkObjectResult Result(dynamic result)
        {
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
