using Pessoas.API.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Pessoas.API.DTOs.Request
{
    public sealed record AdicionarPessoaRequest
    {
        [Required]
        public string Nome { get; init; }

        [Required]
        public DateTime DataNascimento { get; init; }

        [Required]
        public string Cpf { get; set; }

        public string Email { get; set; }
        public string Endereco { get; init; }
        public Sexo? Sexo { get; set; }
        public Nacionalidade? Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
    }

    public sealed record EditarPessoaRequest
    {
        [Required]
        public Guid Id { get; init; }

        [Required]
        public string Nome { get; init; }

        [Required]
        public DateTime DataNascimento { get; init; }

        [Required]
        public string Cpf { get; set; }

        public string Email { get; set; }
        public string Endereco { get; init; }
        public Sexo? Sexo { get; set; }
        public Nacionalidade? Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
    }
}
