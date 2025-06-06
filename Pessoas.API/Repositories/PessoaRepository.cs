﻿using Microsoft.EntityFrameworkCore;
using Pessoas.API.Common;
using Pessoas.API.Exceptions;
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
            var cpfExiste = await _contexto.Pessoas
                .AsNoTracking()
                .AnyAsync(x => x.Cpf == pessoa.Cpf);

            if (cpfExiste)
                throw new DominioInvalidoException("Este CPF já está cadastrado para outra pessoa.");

            _contexto.Pessoas.Add(pessoa);

            await _contexto.SaveChangesAsync();

            return pessoa;
        }

        public async Task<Pessoa> UpdateAsync(Pessoa pessoa)
        {
            var pessoaCpf = await _contexto.Pessoas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Cpf == pessoa.Cpf);

            if (pessoaCpf != null && pessoaCpf.Id != pessoa.Id)
                throw new DominioInvalidoException("Este CPF já está cadastrado para outra pessoa.");

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
