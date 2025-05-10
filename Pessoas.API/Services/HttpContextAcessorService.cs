using Microsoft.AspNetCore.Http;
using Pessoas.API.Services.Interfaces;
using System.Security.Claims;

namespace Pessoas.API.Services
{
    public class HttpContextAcessorService(IHttpContextAccessor context) : IHttpContextAcessorService
    {
        public IHttpContextAccessor HttpContextAccessor { get; } = context;

        public Guid GetId()
        {
            var id = HttpContextAccessor?
                .HttpContext?.User?.FindFirstValue(ClaimTypes.PrimarySid);

            return id != null ? Guid.Parse(id) : Guid.Empty;
        }

        public GetInfoUsuario GetPermissoes()
        {
            if (HttpContextAccessor.HttpContext?.User?.Claims != null)
            {
                string id = HttpContextAccessor.HttpContext?.User?.Claims
                    .Where(x => x.Type == ClaimTypes.PrimarySid)
                    .Select(x => x.Value)
                    .FirstOrDefault();

                string email = HttpContextAccessor.HttpContext?.User?.Claims
                    .Where(x => x.Type == ClaimTypes.Email)
                    .Select(x => x.Value)
                    .FirstOrDefault();

                var permissoes = HttpContextAccessor.HttpContext.User.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToArray();

                var info = new GetInfoUsuario
                {
                    Id = Guid.Parse(id),
                    Email = email,
                    Roles = permissoes
                };

                return info;
            }

            return new GetInfoUsuario
            {
                Id = Guid.Empty,
                Email = string.Empty,
                Roles = new string[] { }
            };
        }

        public bool IsAuthenticated()
        {
            return HttpContextAccessor?
                .HttpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
