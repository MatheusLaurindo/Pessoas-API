using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pessoas.API.DTOs.Request;
using Pessoas.API.Services.Interfaces;

namespace Pessoas.API.Controllers
{
    /// <summary>
    /// Endpoints relacionados à autenticação e permissões de usuários.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(IHttpContextAcessorService httpContext, IAuthService authService) : ControllerBase
    {
        private readonly IHttpContextAcessorService _httpContext = httpContext;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Realiza o login do usuário e insere um token JWT no HTTP Cookies
        /// </summary>
        /// <param name="request">Credenciais de login do usuário.</param>
        /// <returns>Status da autenticação.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="400">Credenciais inválidas ou erro de validação.</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Authenticate(request.Email, request.Senha);

            if (!result.FoiSucesso)
                return BadRequest(result.Mensagem);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddHours(8),
                SameSite = SameSiteMode.Strict
            };

            HttpContext.Response.Cookies.Append("jwt_token", result.Valor.JWT_TOKEN, cookieOptions);

            return Ok();
        }
    }
}
