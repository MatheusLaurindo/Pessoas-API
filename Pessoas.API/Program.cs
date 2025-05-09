using Microsoft.EntityFrameworkCore;
using Pessoas.API.Infra;
using Pessoas.API.Repositories;
using Pessoas.API.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("InMemoryDatabase"));

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Pessoas.API", Version = "v1" });
    options.SwaggerDoc("v2", new() { Title = "Pessoas.API", Version = "v2" });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
