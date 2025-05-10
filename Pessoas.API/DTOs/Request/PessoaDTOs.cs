using Pessoas.API.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Pessoas.API.DTOs.Request
{
    public sealed record AdicionarPessoaRequest
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string Cpf { get; set; }

        public string Email { get; set; }
        public string Endereco { get; set; }
        public Sexo? Sexo { get; set; }
        public Nacionalidade? Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
    }

    public sealed record AdicionarPessoaRequestV2
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O parametro 'Endereço' é obrigatório")]
        public string Endereco { get; set; }

        public string Email { get; set; }
        public Sexo? Sexo { get; set; }
        public Nacionalidade? Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
    }

    public sealed record EditarPessoaRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string Cpf { get; set; }

        public string Email { get; set; }
        public string Endereco { get; set; }
        public Sexo? Sexo { get; set; }
        public Nacionalidade? Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
    }

    public sealed record EditarPessoaRequestV2
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O parametro 'Endereço' é obrigatório")]
        public string Endereco { get; set; }

        public string Email { get; set; }
        public Sexo? Sexo { get; set; }
        public Nacionalidade? Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
    }
}
