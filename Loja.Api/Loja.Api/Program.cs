using Loja.Api;
using Loja.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddCors();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();


// Injeção de dependências centralizada
builder.Services.InjetarDependencias(builder.Configuration);

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowBlazorApp");

// Adicionar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Adicionar todos os endpoints organizados
app.AdicionarTodosEndpoints();

app.MapControllers();
app.MapGet("/", () => "API da Loja Online funcionando!");




app.Run();