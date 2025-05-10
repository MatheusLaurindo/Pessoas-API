using Pessoas.API.Repositories.Interfaces;
using Pessoas.API.Repositories;
using Pessoas.API.Services.Interfaces;
using Pessoas.API.Services;

namespace Pessoas.API.Infra.Installers
{
    public class DependencyInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddHttpContextAccessor();
            services.AddScoped<IHttpContextAcessorService, HttpContextAcessorService>();
        }
    }
}
