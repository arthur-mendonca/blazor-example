using Loja.Api;
using Loja.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Injeção de dependências centralizada
builder.Services.InjetarDependencias(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Adicionar todos os endpoints organizados
app.AdicionarTodosEndpoints();

app.MapControllers();
app.MapGet("/", () => "API da Loja Online funcionando!");
app.Run();