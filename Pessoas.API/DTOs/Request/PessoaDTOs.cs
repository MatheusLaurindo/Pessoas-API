using Pessoas.API.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Pessoas.API.DTOs.Request
{
    public sealed record AdicionarPessoaRequest
    {
        [Required]
        public string Nome { get; init; } = null;

        [Required]
        public DateTime DataNascimento { get; init; }

        [Required]
        public string Cpf { get; init; } = null;

        public string Email { get; init; } = null;
        public string Endereco { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }

    public sealed record AdicionarPessoaRequestV2
    {
        [Required]
        public string Nome { get; init; } = null;

        [Required]
        public DateTime DataNascimento { get; init; }

        [Required]
        public string Cpf { get; init; } = null;

        [Required(ErrorMessage = "O parametro 'Endereço' é obrigatório")]
        public string Endereco { get; init; } = null;

        public string Email { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }

    public sealed record EditarPessoaRequest
    {
        [Required]
        public Guid Id { get; init; } = Guid.Empty;

        [Required]
        public string Nome { get; init; } = null;

        [Required]
        public DateTime DataNascimento { get; init; }

        [Required]
        public string Cpf { get; init; } = null;

        public string Email { get; init; } = null;
        public string Endereco { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }

    public sealed record EditarPessoaRequestV2
    {
        [Required]
        public Guid Id { get; init; } = Guid.Empty;

        [Required]
        public string Nome { get; init; } = null;

        [Required]
        public DateTime DataNascimento { get; init; }

        [Required]
        public string Cpf { get; init; } = null;

        [Required(ErrorMessage = "O parametro 'Endereço' é obrigatório")]
        public string Endereco { get; init; } = null;

        public string Email { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }
}
