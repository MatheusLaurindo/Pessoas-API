using Microsoft.AspNetCore.Mvc;
using Pessoas.API.DTOs.Request;
using Pessoas.API.Services.Interfaces;

namespace Pessoas.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PessoaController(IPessoaService service) : ControllerBase
    {
        private readonly IPessoaService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pessoas = await _service.GetAllAsync();

            return Ok(pessoas);
        }

        [HttpGet("paginado")]
        public async Task<IActionResult> GetPaginated(int pagina = 1, int linhasPorPagina = 10)
        {
            var pessoas = await _service.GetAllPaginatedAsync(pagina, linhasPorPagina);

            return Ok(pessoas);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pessoa = await _service.GetByIdAsync(id);

            if (pessoa == null)
                return NotFound();

            return Ok(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AdicionarPessoaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(request);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EditarPessoaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pessoa = await _service.GetByIdAsync(request.Id);

            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            var result = await _service.UpdateAsync(request);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var pessoa = await _service.GetByIdAsync(id);

            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            var result = await _service.DeleteAsync(id);

            if (!result)
                return BadRequest("Erro ao deletar a pessoa.");

            return Ok("Pessoa deletada com sucesso.");
        }
    }
}
