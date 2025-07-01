using Loja.Models;

namespace Loja.Infra.Produtos
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterTodosProdutos();
        Task<Produto?> ObterProdutoPorId(int id);
        Task<IEnumerable<Produto>> ObterProdutosPorCategoria(string categoria);
        Task AdicionarProduto(Produto produto);
        Task AtualizarProduto(Produto produto);
        Task DeletarProduto(int id);
    }
}
