var builder = WebApplication.CreateBuilder(args);

// Configurar servi�os
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisi��o HTTP
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = ""; // Deixa o Swagger na raiz
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
