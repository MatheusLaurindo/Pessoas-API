using System.ComponentModel.DataAnnotations;

namespace Pessoas.API.DTOs.Request
{
    public sealed record LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
