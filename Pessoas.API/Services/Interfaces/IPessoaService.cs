using Pessoas.API.Common;
using Pessoas.API.DTOs.Request;
using Pessoas.API.DTOs.Response;
using Pessoas.API.Model;

namespace Pessoas.API.Services.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<GetPessoaResp>> GetAllAsync();
        Task<IEnumerable<GetPessoaResp>> GetAllPaginatedAsync(int pagina, int linhasPorPagina);
        Task<GetPessoaResp> GetByIdAsync(Guid id);
        Task<Result<GetPessoaResp>> AddAsync(AdicionarPessoaRequest pessoa);
        Task<Result<GetPessoaResp>> UpdateAsync(EditarPessoaRequest pessoa);
        Task<Result<Guid>> DeleteAsync(Guid id);
    }
}
