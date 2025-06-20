using System.Diagnostics;
using System.Reflection;

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

        private static async Task<IResult> ObterTodosProdutos()
        {
            try
            {
                // TODO: Implementar quando criar ProdutoUseCase
                return TypedResults.Ok(new { message = "Endpoint de produtos em desenvolvimento" });
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

        private static async Task<IResult> ObterProdutoPorId(int id)
        {
            try
            {
                // TODO: Implementar quando criar ProdutoUseCase
                return TypedResults.Ok(new { message = $"Produto {id} em desenvolvimento" });
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

        private static async Task<IResult> ObterProdutosPorCategoria(string categoria)
        {
            try
            {
                // TODO: Implementar quando criar ProdutoUseCase
                return TypedResults.Ok(new { message = $"Produtos da categoria {categoria} em desenvolvimento" });
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

        private static async Task<IResult> CriarProduto()
        {
            try
            {
                // TODO: Implementar quando criar ProdutoUseCase
                return TypedResults.Ok(new { message = "Criar produto em desenvolvimento" });
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

        private static async Task<IResult> AlterarProduto(int id)
        {
            try
            {
                // TODO: Implementar quando criar ProdutoUseCase
                return TypedResults.Ok(new { message = $"Alterar produto {id} em desenvolvimento" });
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

        private static async Task<IResult> ExcluirProduto(int id)
        {
            try
            {
                // TODO: Implementar quando criar ProdutoUseCase
                return TypedResults.Ok(new { message = $"Excluir produto {id} em desenvolvimento" });
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