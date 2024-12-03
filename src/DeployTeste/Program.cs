using DeployTeste.Repositories;
using FirebirdSql.Data.FirebirdClient;

var builder = WebApplication.CreateBuilder(args);

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
    builder.WithOrigins("https://deployteste-9od7.onrender.com/api").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configurar Swagger para produção com restrição
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Define um endpoint seguro (ex.: apenas usuários autenticados)
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Teste v1");
    options.RoutePrefix = ""; // Swagger disponível na raiz
});

// Aplica a configuração de CORS
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();
