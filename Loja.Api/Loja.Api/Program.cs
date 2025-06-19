using Loja.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Loja.UseCases.Usuarios;
using Loja.Infra.Usuarios;
using Loja.Infra.Pedidos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();


builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("Loja.Api")));

builder.Services.AddScoped<IPedidoDAO, PedidoDAO>();
builder.Services.AddScoped<IUsuarioDAO, UsuarioDAO>();
builder.Services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.MapGet("/", () => "API da Loja Online funcionando!");
app.Run();