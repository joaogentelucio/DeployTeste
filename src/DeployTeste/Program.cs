using DeployTeste.Repositories;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Lê as variáveis de ambiente
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var connectionString = Environment.GetEnvironmentVariable("FirebirdConnection");

// Configura a string de conexão no builder
builder.Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables() // Lê as variáveis de ambiente
    .Build());

// Registra a conexão do Firebird como serviço
builder.Services.AddScoped<FbConnection>(serviceProvider =>
    new FbConnection(connectionString));

// Registra o repositório de usuários
builder.Services.AddScoped<UsuariosRepository>();

// Add services to the container
builder.Services.AddControllers();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração de CORS
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplica a configuração de CORS
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();
