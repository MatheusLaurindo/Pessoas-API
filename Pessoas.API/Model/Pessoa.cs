using Pessoas.API.Enuns;
using Pessoas.API.Exceptions;
using Pessoas.API.Model.Base;
using System.Text.RegularExpressions;

namespace Pessoas.API.Model
{
    public class Pessoa : Entidade
    {
        public string Nome { get; protected set; }
        public string Email { get; protected set; }
        public DateTime DataNascimento { get; protected set; }
        public string Cpf { get; protected set; }
        public string Endereco { get; set; }
        public Sexo? Sexo { get; protected set; }
        public Nacionalidade? Nacionalidade { get; protected set; }
        public string Naturalidade { get; protected set; }

        protected Pessoa()
            : base()
        {
        }

        protected Pessoa(string nome, string email, DateTime dataNascimento, string cpf, Sexo? sexo, Nacionalidade? nacionalidade, string naturalidade, string endereco)
            : this()
        {
            SetNome(nome);
            SetEmail(email);
            SetDataNascimento(dataNascimento);
            SetCpf(cpf);
            SetEndereco(endereco);
            SetSexo(sexo);
            SetNacionalidade(nacionalidade);
            SetNaturalidade(naturalidade);
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DominioInvalidoException(nameof(nome));

            if (nome.Length > 100)
                throw new DominioInvalidoException(nameof(nome));

            Nome = nome;
        }

        public void SetEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length > 255)
                    throw new DominioInvalidoException(nameof(email));

                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

                if (!regex.IsMatch(email))
                    throw new DominioInvalidoException(nameof(email));
            }

            Email = email;
        }

        public void SetDataNascimento(DateTime dataNascimento)
        {
            if (dataNascimento > DateTime.UtcNow)
                throw new DominioInvalidoException(nameof(dataNascimento));

            DataNascimento = dataNascimento;
        }

        public void SetCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new DominioInvalidoException(nameof(cpf));

            if (cpf.Length < 14)
                throw new DominioInvalidoException(nameof(cpf));

            Cpf = cpf;
        }

        public void SetEndereco(string endereco)
        {
            if (!string.IsNullOrWhiteSpace(endereco))
                if (endereco.Length > 255)
                    throw new DominioInvalidoException(nameof(endereco));

            Endereco = endereco;
        }

        public void SetSexo(Sexo? sexo)
        {
            Sexo = sexo;
        }

        public void SetNacionalidade(Nacionalidade? nacionalidade)
        {
            Nacionalidade = nacionalidade;
        }

        public void SetNaturalidade(string naturalidade)
        {
            Naturalidade = naturalidade;
        }

        public static Pessoa Create(
            string nome, 
            string email, 
            DateTime dataNascimento, 
            string cpf, 
            Sexo? sexo, 
            Nacionalidade? nacionalidade, 
            string naturalidade, 
            string endereco)
        {
            return new Pessoa(nome, email, dataNascimento, cpf, sexo, nacionalidade, naturalidade, endereco);
        }
    }
}
