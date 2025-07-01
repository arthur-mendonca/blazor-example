using System.Diagnostics;
using System.Reflection;
using Loja.Infra.Data;
using Loja.Models;
using Loja.UseCases.Produtos;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Endpoints
{
    public static class AdicionarEndpointsProdutosExtensions
    {
        /// <summary>
        /// Método para adicionar os endpoints de produtos da API.
        /// </summary>
        /// <param name="app">Instância do WebApplication.</param>
        public static void AdicionarEndpointsProdutos(this WebApplication app)
        {
            var produtos = app.MapGroup("/api/produtos")
                .WithTags("Produtos")
                .WithDescription("Endpoints relacionados a produtos");

            produtos.MapGet("/", ObterTodosProdutos)
                .WithName("Obter todos os produtos")
                .WithSummary("Retorna todos os produtos do sistema");

            produtos.MapGet("/{id}", ObterProdutoPorId)
                .WithName("Obter produto por ID")
                .WithSummary("Retorna um produto específico pelo ID");

            produtos.MapGet("/categoria/{categoria}", ObterProdutosPorCategoria)
                .WithName("Obter produtos por categoria")
                .WithSummary("Retorna produtos de uma categoria específica");

            produtos.MapPost("/", CriarProduto)
                .WithName("Criar produto")
                .WithSummary("Cria um novo produto no sistema")
                .RequireAuthorization();

            produtos.MapPut("/{id}", AlterarProduto)
                .WithName("Alterar produto")
                .WithSummary("Altera um produto existente")
                .RequireAuthorization();

            produtos.MapDelete("/{id}", ExcluirProduto)
                .WithName("Excluir produto")
                .WithSummary("Exclui um produto do sistema")
                .RequireAuthorization();
        }

        private static async Task<IResult> ObterTodosProdutos(LojaDbContext dbContext)
        {
            try
            {
                // Query direta via SQL para evitar problemas com o DbSet
                var produtos = await dbContext.Database.SqlQueryRaw<Produto>("SELECT * FROM Produto").ToListAsync();
                return TypedResults.Ok(produtos);
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }
        private static async Task<IResult> ObterProdutoPorId(int id, LojaDbContext dbContext)
        {
            try
            {
                // Query direta via SQL para evitar problemas com o DbSet
                var produto = await dbContext.Database.SqlQueryRaw<Produto>($"SELECT * FROM Produto WHERE Id = {id} LIMIT 1").FirstOrDefaultAsync();
                if (produto == null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(produto);
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> ObterProdutosPorCategoria(string categoria, LojaDbContext dbContext)
        {
            try
            {
                // Query direta via SQL para evitar problemas com o DbSet
                var produtos = await dbContext.Database.SqlQueryRaw<Produto>($"SELECT * FROM Produto WHERE Categoria = '{categoria}'").ToListAsync();
                return TypedResults.Ok(produtos);
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> CriarProduto(Produto produto, IProdutoUseCase produtoUseCase)
        {
            try
            {
                var novoProduto = await produtoUseCase.CriarProduto(produto);
                return TypedResults.Created($"/api/produtos/{novoProduto.Id}", novoProduto);
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> AlterarProduto(int id, Produto produto, IProdutoUseCase produtoUseCase)
        {
            try
            {
                await produtoUseCase.AlterarProduto(id, produto);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> ExcluirProduto(int id, IProdutoUseCase produtoUseCase)
        {
            try
            {
                await produtoUseCase.ExcluirProduto(id);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }
    }
}