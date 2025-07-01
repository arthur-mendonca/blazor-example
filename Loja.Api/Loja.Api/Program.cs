using Loja.Api;
using Loja.Api.Endpoints;
using Loja.Infra.Data;
using Loja.Models;
using Microsoft.EntityFrameworkCore;


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

// Seed de produtos para teste
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LojaDbContext>();
    SeedProdutos(dbContext);
}

app.Run();

// Método para adicionar produtos de exemplo ao banco de dados
void SeedProdutos(LojaDbContext dbContext)
{
    try
    {
        // Garantir que o banco de dados existe
        dbContext.Database.EnsureCreated();

        // Verificar se já existem produtos de forma correta
        var produtosExistentes = dbContext.Produtos.Any();

        // Se não há produtos, inserir dados iniciais
        if (!produtosExistentes)
        {
            var produtos = new List<Produto>
            {
                new Produto
                {
                    Nome = "Notebook Dell XPS 15",
                    Preco = 7999.99M,
                    Imagem = "/img/laptop.png",
                    Categoria = "Computador",
                    Descricao = "Notebook premium com tela 4K, Intel Core i7, 16GB RAM, SSD 512GB",
                    EstoqueQuantidade = 10,
                    Ativo = true
                },
                new Produto
                {
                    Nome = "iPhone 15 Pro",
                    Preco = 8499.90M,
                    Imagem = "/img/cel-mockup.png",
                    Categoria = "Celular",
                    Descricao = "Smartphone com câmera de 48MP, chip A17 Pro, 256GB de armazenamento",
                    EstoqueQuantidade = 15,
                    Ativo = true
                },
                new Produto
                {
                    Nome = "iPad Pro M2",
                    Preco = 9799.00M,
                    Imagem = "/img/tablet-mockup.png",
                    Categoria = "Tablet",
                    Descricao = "Tablet com chip M2, tela Liquid Retina XDR de 11 polegadas, 128GB",
                    EstoqueQuantidade = 8,
                    Ativo = true
                },
                new Produto
                {
                    Nome = "Mouse Logitech MX Master 3",
                    Preco = 699.90M,
                    Imagem = "/img/mouse-mockup.png",
                    Categoria = "Acessorio",
                    Descricao = "Mouse sem fio com sensor Darkfield de 4000 DPI, conexão Bluetooth",
                    EstoqueQuantidade = 30,
                    Ativo = true
                },
                new Produto
                {
                    Nome = "Teclado Mecânico Keychron K2",
                    Preco = 799.90M,
                    Imagem = "/img/pais.png",
                    Categoria = "Acessorio",
                    Descricao = "Teclado mecânico sem fio com switches Gateron Brown, layout ABNT2",
                    EstoqueQuantidade = 12,
                    Ativo = true
                }
            };

            dbContext.Produtos.AddRange(produtos);
            dbContext.SaveChanges();

            Console.WriteLine("Produtos de exemplo adicionados com sucesso!");
        }
        else
        {
            Console.WriteLine("A tabela Produto já contém dados, pulando o seed.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao adicionar produtos: {ex.Message}");
    }
}