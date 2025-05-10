using Microsoft.AspNetCore.Mvc;
using Pessoas.API.Atributos;
using Pessoas.API.DTOs.Request;
using Pessoas.API.Enuns;
using Pessoas.API.Services.Interfaces;
using System.Net.Mime;

namespace Pessoas.API.Controllers
{
    public abstract class PessoaBaseController(IPessoaService service) : ControllerBase
    {
        private readonly IPessoaService _service = service;

        /// <summary>
        /// Retorna todas as pessoas.
        /// </summary>
        [AppAuthorize(Permissao.Visualizar_Pessoa)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var pessoas = await _service.GetAllAsync();

            return Ok(pessoas);
        }

        /// <summary>
        /// Retorna pessoas de forma paginada.
        /// </summary>
        /// <param name="pagina">Número da página.</param>
        /// <param name="linhasPorPagina">Quantidade de registros por página.</param>
        [AppAuthorize(Permissao.Visualizar_Pessoa)]
        [HttpGet("paginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaginatedAsync(int pagina = 1, int linhasPorPagina = 10)
        {
            var pessoas = await _service.GetAllPaginatedAsync(pagina, linhasPorPagina);

            return Ok(pessoas);
        }

        /// <summary>
        /// Retorna uma pessoa pelo ID.
        /// </summary>
        /// <param name="id">ID da pessoa.</param>
        [AppAuthorize(Permissao.Visualizar_Pessoa)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var pessoa = await _service.GetByIdAsync(id);

            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            return Ok(pessoa);
        }

        /// <summary>
        /// Remove uma pessoa pelo ID.
        /// </summary>
        /// <param name="id">ID da pessoa a ser removida.</param>
        [AppAuthorize(Permissao.Remover_Pessoa)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var pessoa = await _service.GetByIdAsync(id);

            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            var result = await _service.DeleteAsync(id);

            if (!result.FoiSucesso)
            {
                ModelState.AddModelError("", result.Mensagem);
                return BadRequest(result.Mensagem);
            }

            return Ok(result);
        }
    }
}
