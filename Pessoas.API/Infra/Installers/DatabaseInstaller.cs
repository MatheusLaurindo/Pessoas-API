using Microsoft.EntityFrameworkCore;

namespace Pessoas.API.Infra.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("InMemoryDatabase"));
        }
    }
}
