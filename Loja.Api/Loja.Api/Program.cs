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

        // Criar a tabela Produto manualmente, se não existir
        dbContext.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS Produto (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Preco DECIMAL(10,2) NOT NULL,
                Imagem TEXT,
                Categoria TEXT NOT NULL,
                Descricao TEXT,
                EstoqueQuantidade INTEGER NOT NULL,
                Ativo INTEGER NOT NULL
            )");

        // Verificar se já existem produtos
        var count = dbContext.Database.ExecuteSqlRaw("SELECT COUNT(*) FROM Produto");

        // Se não há produtos, inserir dados iniciais
        if (count == 0)
        {
            // Inserir diretamente via SQL para evitar problemas com o DbSet
            dbContext.Database.ExecuteSqlRaw(@"
                INSERT INTO Produto (Nome, Preco, Imagem, Categoria, Descricao, EstoqueQuantidade, Ativo)
                VALUES 
                ('Notebook Dell XPS 15', 7999.99, '/img/laptop.png', 'Computador', 'Notebook premium com tela 4K, Intel Core i7, 16GB RAM, SSD 512GB', 10, 1),
                ('iPhone 15 Pro', 8499.90, '/img/cel-mockup.png', 'Celular', 'Smartphone com câmera de 48MP, chip A17 Pro, 256GB de armazenamento', 15, 1),
                ('iPad Pro M2', 9799.00, '/img/tablet-mockup.png', 'Tablet', 'Tablet com chip M2, tela Liquid Retina XDR de 11 polegadas, 128GB', 8, 1),
                ('Mouse Logitech MX Master 3', 699.90, '/img/mouse-mockup.png', 'Acessorio', 'Mouse sem fio com sensor Darkfield de 4000 DPI, conexão Bluetooth', 30, 1),
                ('Teclado Mecânico Keychron K2', 799.90, '/img/pais.png', 'Acessorio', 'Teclado mecânico sem fio com switches Gateron Brown, layout ABNT2', 12, 1)
            ");

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