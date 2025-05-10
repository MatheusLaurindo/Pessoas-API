using Microsoft.AspNetCore.Mvc;
using Pessoas.API.DTOs.Request;
using Pessoas.API.Services.Interfaces;
using System.Net.Mime;

namespace Pessoas.API.Controllers;

/// <summary>
/// Gerencia operações relacionadas a pessoas.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PessoaController(IPessoaService service) : ControllerBase
{
    private readonly IPessoaService _service = service;

    /// <summary>
    /// Retorna todas as pessoas.
    /// </summary>
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
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var pessoa = await _service.GetByIdAsync(id);
        return pessoa is null ? NotFound() : Ok(pessoa);
    }

    /// <summary>
    /// Adiciona uma nova pessoa.
    /// </summary>
    /// <param name="request">Dados da nova pessoa.</param>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAsync([FromBody] AdicionarPessoaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.AddAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Atualiza uma pessoa existente.
    /// </summary>
    /// <param name="request">Dados atualizados da pessoa.</param>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromBody] EditarPessoaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pessoa = await _service.GetByIdAsync(request.Id);
        if (pessoa == null)
            return NotFound("Pessoa não encontrada.");

        var result = await _service.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Remove uma pessoa pelo ID.
    /// </summary>
    /// <param name="id">ID da pessoa a ser removida.</param>
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
        return result ? Ok("Pessoa deletada com sucesso.") : BadRequest("Erro ao deletar a pessoa.");
    }
}
