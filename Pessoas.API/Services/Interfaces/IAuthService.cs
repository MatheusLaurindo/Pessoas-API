using Pessoas.API.Common;
using Pessoas.API.Utils;

namespace Pessoas.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<JwtToken>> Authenticate(string email, string senha);
    }
}
