using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pessoas.API.Atributos;
using Pessoas.API.Common;
using Pessoas.API.DTOs.Request;
using Pessoas.API.Enuns;
using Pessoas.API.Model;
using Pessoas.API.Services.Interfaces;
using System.Net.Mime;

namespace Pessoas.API.Controllers;

/// <summary>  
/// Gerencia operações relacionadas a pessoas.  
/// </summary>  
[Authorize]
[ApiController]
[Route("api/v1/pessoa")]
[ApiExplorerSettings(GroupName = "v1")]
[Produces(MediaTypeNames.Application.Json)]
public class PessoaController : PessoaBaseController
{
    private readonly IPessoaService _service;

    public PessoaController(IPessoaService service) : base(service)
    {
        _service = service;
    }

    /// <summary>
    /// Adiciona uma nova pessoa.
    /// </summary>
    /// <param name="request">Dados da nova pessoa.</param>
    [AppAuthorize(Permissao.Adicionar_Pessoa)]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAsync([FromBody] AdicionarPessoaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.AddAsync(request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);
            return BadRequest(APITypedResponse<Result<Pessoa>>.Create(result, false, result.Mensagem));
        }

        return Ok(APITypedResponse<Result<Pessoa>>.Create(result, true, result.Mensagem));
    }

    /// <summary>
    /// Atualiza uma pessoa existente.
    /// </summary>
    /// <param name="request">Dados atualizados da pessoa.</param>
    [AppAuthorize(Permissao.Editar_Pessoa)]
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
            return NotFound(APITypedResponse<Result<Pessoa>>.Create(null, false, "Pessoa não encontrada."));

        var result = await _service.UpdateAsync(request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);
            return BadRequest(APITypedResponse<Result<Pessoa>>.Create(result, false, result.Mensagem));
        }

        return Ok(APITypedResponse<Result<Pessoa>>.Create(result, true, result.Mensagem));
    }
}

/// <summary>  
/// Gerencia operações relacionadas a pessoas. (V2)
/// </summary>  
[Authorize]
[ApiController]
[Route("api/v2/pessoa")]
[ApiExplorerSettings(GroupName = "v2")]
[Produces(MediaTypeNames.Application.Json)]
public class PessoaControllerV2 : PessoaBaseController
{
    private readonly IPessoaService _service;

    public PessoaControllerV2(IPessoaService service) : base(service)
    {
        _service = service;
    }


    /// <summary>
    /// Adiciona uma nova pessoa. (V2)
    /// </summary>
    /// <param name="request">Dados da nova pessoa. O campo <c>Endereço</c> é obrigatório.</param>
    [AppAuthorize(Permissao.Adicionar_Pessoa)]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAsync([FromBody] AdicionarPessoaRequestV2 request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var v1Request = new AdicionarPessoaRequest
        {
            Nome = request.Nome,
            DataNascimento = request.DataNascimento,
            Cpf = request.Cpf,
            Email = request.Email,
            Endereco = request.Endereco,
            Sexo = request.Sexo,
            Nacionalidade = request.Nacionalidade,
            Naturalidade = request.Naturalidade
        };

        var result = await _service.AddAsync(v1Request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);
            return BadRequest(APITypedResponse<Result<Pessoa>>.Create(result, false, result.Mensagem));
        }

        return Ok(APITypedResponse<Result<Pessoa>>.Create(result, true, result.Mensagem));
    }

    /// <summary>
    /// Atualiza uma pessoa existente. (V2)
    /// </summary>
    /// <param name="request">Dados atualizados da pessoa. O campo <c>Endereço</c> é obrigatório.</param>
    [AppAuthorize(Permissao.Editar_Pessoa)]
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromBody] EditarPessoaRequestV2 request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pessoa = await _service.GetByIdAsync(request.Id);

        if (pessoa == null)
            return NotFound(APITypedResponse<Result<Pessoa>>.Create(null, false, "Pessoa não encontrada."));

        var v1Request = new EditarPessoaRequest
        {
            Nome = request.Nome,
            DataNascimento = request.DataNascimento,
            Cpf = request.Cpf,
            Email = request.Email,
            Endereco = request.Endereco,
            Sexo = request.Sexo,
            Nacionalidade = request.Nacionalidade,
            Naturalidade = request.Naturalidade
        };

        var result = await _service.UpdateAsync(v1Request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);
            return BadRequest(APITypedResponse<Result<Pessoa>>.Create(result, false, result.Mensagem));
        }

        return Ok(APITypedResponse<Result<Pessoa>>.Create(result, true, result.Mensagem));
    }
}
