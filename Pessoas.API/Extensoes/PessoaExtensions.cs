using Pessoas.API.DTOs.Response;
using Pessoas.API.Model;

namespace Pessoas.API.Extensoes
{
    public static class PessoaExtensions
    {
        public static GetPessoaResp ToDto(this Pessoa p) =>
            new()
            {
                Id = p.Id,
                Nome = p.Nome,
                Email = p.Email,
                DataNascimento = p.DataNascimento,
                Cpf = p.Cpf,
                Sexo = p.Sexo.ToString(),
                Nacionalidade = p.Nacionalidade.ToString(),
                Naturalidade = p.Naturalidade,
                Endereco = p.Endereco
            };
    }
}
