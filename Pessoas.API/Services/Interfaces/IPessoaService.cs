using Pessoas.API.DTOs.Request;
using Pessoas.API.DTOs.Response;

namespace Pessoas.API.Services.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<GetPessoaResp>> GetAllAsync();
        Task<IEnumerable<GetPessoaResp>> GetAllPaginatedAsync(int pagina, int linhasPorPagina);
        Task<GetPessoaResp> GetByIdAsync(Guid id);
        Task<GetPessoaResp> AddAsync(AdicionarPessoaRequest pessoa);
        Task<GetPessoaResp> UpdateAsync(EditarPessoaRequest pessoa);
        Task<bool> DeleteAsync(Guid id);
    }
}
