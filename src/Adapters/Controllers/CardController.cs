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
    public class CardController : ControllerBase
    {
        private readonly IServiceCard _service;
        private readonly IAntiforgery _antiforgery;

        public CardController(IServiceCard service, IAntiforgery antiforgery)
        {
            _service = service;
            _antiforgery = antiforgery;
        }

        // GET: api/Card
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet]
        public async Task<ActionResult> GetCards([FromBody] Card data)
        {
            IResultadoOperacao<List<Card>> result = await _service.Search(data);
            return Result(result);
        }

        // GET: api/Card
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCard(int id, [FromBody] Card data)
        {
            if (id == data.CardId)
            {
                IResultadoOperacao<Card> result = await _service.GetOne(data);
                return Result(result);
            }
            return NotFound("Card não existe");
        }

        // PUT: api/Card
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCard(int id, [FromBody] Card data)
        {
            if (id == data.CardId)
            {
                IResultadoOperacao<Card> result = await _service.Edit(data);
                return Result(result);
            }
            return NotFound("Card não existe");
        }

        // POST: api/Card
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPost]
        public async Task<ActionResult> PostCard([FromBody] Card data)
        {
            IResultadoOperacao<Card> result = await _service.Create(data);
            return Result(result);
        }

        // DELETE: api/Card
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCard(int id, [FromBody] Card data)
        {
            if (id == data.CardId)
            {
                IResultadoOperacao<Card> result = await _service.Delete(data);
                return Result(result);
            }
            return NotFound("Card não existe");
        }

        // PATCH: api/Card/AlterType
        [Authorize]
        [RequireAntiforgeryToken]
        [HttpPatch("AlterType/{id}")]
        public async Task<ActionResult> AlterTypeCard(int id, [FromBody] Card data)
        {
            if (id == data.CardId)
            {
                IResultadoOperacao<Card> result = await _service.AlterType(data);
                return Result(result);
            }
            return NotFound("Card não existe");
        }

        private OkObjectResult Result(dynamic result)
        {
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }
    }
}
