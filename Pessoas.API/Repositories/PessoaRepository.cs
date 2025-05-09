using Microsoft.EntityFrameworkCore;
using Pessoas.API.Infra;
using Pessoas.API.Model;
using Pessoas.API.Repositories.Interfaces;

namespace Pessoas.API.Repositories
{
    public class PessoaRepository(AppDbContext contexto) : IPessoaRepository
    {
        private readonly AppDbContext _contexto = contexto;

        public async Task<IEnumerable<Pessoa>> GetAllAsync()
            => await _contexto.Pessoas
            .AsNoTracking()
            .ToListAsync();

        public async Task<IEnumerable<Pessoa>> GetAllPaginatedAsync(int pagina, int linhasPorPagina)
        {
            var query = _contexto.Pessoas
                .AsNoTracking()
                .OrderByDescending(x => x.DataCadastro)
                .Skip((pagina - 1) * linhasPorPagina)
                .Take(linhasPorPagina);

            return await query.ToListAsync();
        }

        public async Task<Pessoa> GetByIdAsync(Guid id)
            => await _contexto.Pessoas
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Pessoa> AddAsync(Pessoa pessoa)
        {
            _contexto.Pessoas.Add(pessoa);

            await _contexto.SaveChangesAsync();

            return pessoa;
        }

        public async Task<Pessoa> UpdateAsync(Pessoa pessoa)
        {
            pessoa.SetDataAtualizacao();

            _contexto.Pessoas.Update(pessoa);

            await _contexto.SaveChangesAsync();

            return pessoa;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var pessoa = await _contexto.Pessoas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pessoa == null)
                return false;

            _contexto.Pessoas.Remove(pessoa);

            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}
