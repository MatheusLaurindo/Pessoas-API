using Pessoas.API.Common;
using Pessoas.API.Enuns;
using Pessoas.API.Exceptions;
using Pessoas.API.Model.Base;
using System.Text.RegularExpressions;

namespace Pessoas.API.Model
{
    public class Usuario : Entidade
    {
        public string Email { get; protected set; }
        public string Senha { get; protected set; }
        public ICollection<UsuarioPermissao> VinculosPermissao { get; protected set; }

        protected Usuario()
            : base()
        {
            VinculosPermissao = new List<UsuarioPermissao>();
        }

        protected Usuario(string email, string senha)
            : this()
        {
            SetEmail(email);
            SetSenha(senha);
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

        public void SetSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                throw new DominioInvalidoException(nameof(senha));

            if (senha.Length < 8 && senha.Length > 20)
                throw new DominioInvalidoException(nameof(senha));

            if (!senha.Any(char.IsDigit))
                throw new DominioInvalidoException(nameof(senha));

            Senha = senha;
        }

        public static Result<Usuario> Create(string email, string senha)
        {
            return Result<Usuario>.Sucesso(new Usuario(email, senha));
        }
    }
}
