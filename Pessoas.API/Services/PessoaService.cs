using Pessoas.API.DTOs.Request;
using Pessoas.API.DTOs.Response;
using Pessoas.API.Extensoes;
using Pessoas.API.Model;
using Pessoas.API.Repositories.Interfaces;
using Pessoas.API.Services.Interfaces;

namespace Pessoas.API.Services
{
    public class PessoaService(IPessoaRepository repository) : IPessoaService
    {
        private readonly IPessoaRepository _repository = repository;

        public async Task<IEnumerable<GetPessoaResp>> GetAllAsync()
        {
            var pessoas = await _repository.GetAllAsync();

            return pessoas.Select(p => p.ToDto());
        }

        public async Task<IEnumerable<GetPessoaResp>> GetAllPaginatedAsync(int pagina, int linhasPorPagina)
        {
            var pessoas = await _repository.GetAllPaginatedAsync(pagina, linhasPorPagina);

            return pessoas.Select(p => p.ToDto());
        }

        public async Task<GetPessoaResp> GetByIdAsync(Guid id)
        {
            var pessoa = await _repository.GetByIdAsync(id);

            if (pessoa == null)
                return null;

            return pessoa.ToDto();
        }

        public async Task<GetPessoaResp> AddAsync(AdicionarPessoaRequest pessoa)
        {
            try
            {
                var novaPessoa = Pessoa.Create(
                pessoa.Nome,
                pessoa.Email,
                pessoa.DataNascimento,
                pessoa.Cpf,
                pessoa.Sexo,
                pessoa.Nacionalidade,
                pessoa.Naturalidade,
                pessoa.Endereco);

                var result = await _repository.AddAsync(novaPessoa);

                return result.ToDto();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetPessoaResp> UpdateAsync(EditarPessoaRequest pessoa)
        {
            try
            {
                var pessoaExistente = await _repository.GetByIdAsync(pessoa.Id);

                if (pessoaExistente == null)
                    return null;

                pessoaExistente.SetNome(pessoa.Nome);
                pessoaExistente.SetEmail(pessoa.Email);
                pessoaExistente.SetDataNascimento(pessoa.DataNascimento);
                pessoaExistente.SetCpf(pessoa.Cpf);
                pessoaExistente.SetEndereco(pessoa.Endereco);
                pessoaExistente.SetSexo(pessoa.Sexo);
                pessoaExistente.SetNacionalidade(pessoa.Nacionalidade);
                pessoaExistente.SetNaturalidade(pessoa.Naturalidade);

                var result = await _repository.UpdateAsync(pessoaExistente);

                return result.ToDto();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var pessoaExistente = await _repository.GetByIdAsync(id);

                if (pessoaExistente == null)
                    return false;

                return await _repository.DeleteAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
