using Loja.Models;

namespace Loja.UseCases.Produtos
{
    public interface IProdutoUseCase
    {
        Task<IEnumerable<Produto>> ObterTodosProdutos();
        Task<Produto?> ObterProdutoPorId(int id);
        Task<IEnumerable<Produto>> ObterProdutosPorCategoria(string categoria);
        Task<Produto> CriarProduto(Produto produto);
        Task AlterarProduto(int id, Produto produto);
        Task ExcluirProduto(int id);
    }
}
