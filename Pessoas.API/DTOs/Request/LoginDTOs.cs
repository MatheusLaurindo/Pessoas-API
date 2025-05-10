using System.ComponentModel.DataAnnotations;

namespace Pessoas.API.DTOs.Request
{
    public sealed record LoginRequest
    {
        [Required]
        public string Email { get; init; } = null;
        [Required]
        public string Senha { get; init; } = null;
    }
}
