using Microsoft.EntityFrameworkCore;
using Pessoas.API.DTOs.Request;
using Pessoas.API.Enuns;
using Pessoas.API.Infra;
using Pessoas.API.Model;
using Pessoas.API.Repositories;
using Pessoas.API.Services;

namespace Tests.ServiceTests.Pessoa;

public class PessoaServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        Seeding.SeedPessoas(context);

        return context;
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodasAsPessoas()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        // Act
        var resultado = await service.GetAllAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(15, resultado.Count());
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarPessoa_SeIdValido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.API.Model.Pessoa.Create(
            "João",
            "joao@email.com",
            DateTime.Today.AddYears(-30),
            "123.456.789-00",
            Sexo.Masculino,
            Nacionalidade.Brasileira,
            "SP",
            "Rua A").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.GetByIdAsync(pessoa.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(pessoa.Nome, resultado.Nome);
        Assert.Equal(pessoa.Email, resultado.Email);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNulo_SeIdInvalido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        // Act
        var resultado = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task GetAllPaginatedAsync_DeveRetornarPessoasPaginadas()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        // Act
        var resultado = await service.GetAllPaginatedAsync(1, 5);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(5, resultado.Count());
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarPessoaComSucesso()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "joao@email.com",
            DataNascimento = DateTime.Today.AddYears(-25),
            Cpf = "111.111.111-11",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.True(resultado.FoiSucesso);
        Assert.NotEqual(Guid.Empty, resultado.Valor.Id);
    }

    [Fact]
    public async Task AddAsync_DeveFalhar_SeCpfJaExistir()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "joao@email.com",
            DataNascimento = DateTime.Today.AddYears(-25),
            Cpf = "123.456.789-01",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
    }

    [Fact]
    public async Task AddAsync_DeveFalhar_SeCpfInvalido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "joao@email.com",
            DataNascimento = DateTime.Today.AddYears(-25),
            Cpf = "123123123123",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact]
    public async Task AddAsync_DeveFalhar_SeEmailInvalido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "emailinvalidoteste",
            DataNascimento = DateTime.Today.AddYears(-25),
            Cpf = "123.123.123-12",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact]
    public async Task AddAsync_DeveFalhar_SeNomeInvalido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "",
            Email = "email@gmail.com",
            DataNascimento = DateTime.Today.AddYears(-25),
            Cpf = "123.123.123-12",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact]
    public async Task AddAsync_DeveFalhar_SeDataNascimentoInvalida()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "joao da silva",
            Email = "email@gmail.com",
            DataNascimento = DateTime.UtcNow.AddDays(10),
            Cpf = "123.123.123-12",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarPessoaComSucesso()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.API.Model.Pessoa.Create(
            "Ana", 
            "ana@email.com", 
            DateTime.Today.AddYears(-28), 
            "111.333.555-66", 
            Sexo.Feminino, 
            Nacionalidade.Brasileira, 
            "BA", 
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        var request = new EditarPessoaRequest
        {
            Id = pessoa.Id,
            Nome = "Ana Atualizada",
            Email = "ana@nova.com",
            DataNascimento = pessoa.DataNascimento,
            Cpf = pessoa.Cpf,
            Sexo = pessoa.Sexo,
            Nacionalidade = pessoa.Nacionalidade,
            Naturalidade = pessoa.Naturalidade,
            Endereco = "Rua Nova"
        };

        // Act
        var resultado = await service.UpdateAsync(request);

        // Assert
        Assert.True(resultado.FoiSucesso);
        Assert.Equal(request.Nome, resultado.Valor.Nome);
        Assert.Equal(request.Email, resultado.Valor.Email);
        Assert.Equal(request.Endereco, resultado.Valor.Endereco);
    }

    [Fact]
    public async Task UpdateAsync_DeveFalharEdicao_SeEmailInvalido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.API.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        var request = new EditarPessoaRequest
        {
            Id = pessoa.Id,
            Nome = "Ana Atualizada",
            Email = "ananova.com",
            DataNascimento = pessoa.DataNascimento,
            Cpf = pessoa.Cpf,
            Sexo = pessoa.Sexo,
            Nacionalidade = pessoa.Nacionalidade,
            Naturalidade = pessoa.Naturalidade,
            Endereco = "Rua Nova"
        };

        // Act
        var resultado = await service.UpdateAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact]
    public async Task UpdateAsync_DeveFalharEdicao_SeNomeInvalido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.API.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        var request = new EditarPessoaRequest
        {
            Id = pessoa.Id,
            Nome = "",
            Email = "ana@nova.com",
            DataNascimento = pessoa.DataNascimento,
            Cpf = pessoa.Cpf,
            Sexo = pessoa.Sexo,
            Nacionalidade = pessoa.Nacionalidade,
            Naturalidade = pessoa.Naturalidade,
            Endereco = "Rua Nova"
        };

        // Act
        var resultado = await service.UpdateAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverPessoaComSucesso()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.API.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.DeleteAsync(pessoa.Id);

        // Assert
        Assert.True(resultado.FoiSucesso);
        Assert.Equal(pessoa.Id, resultado.Valor);
    }

    [Fact]
    public async Task DeleteAsync_DeveFalharRemocao_SeIdInvalido()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.API.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.DeleteAsync(Guid.Empty);

        // Assert
        Assert.False(resultado.FoiSucesso);
    }
}
