using Pessoas.API.Infra;
using Pessoas.API.Infra.Extensoes;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Instalar serviços via padrão Installer
builder.Services.InstallServicesFromAssembly(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pessoas API V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Pessoas API V2");
    options.DocumentTitle = "Pessoas.API Docs";
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    Seeding.SeedPessoas(db);
    Seeding.SeedUsuarios(db);
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
