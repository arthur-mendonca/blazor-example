using Loja.Infra.Produtos;
using Loja.Models;
using Loja.UseCases.Produtos;

namespace Loja.UseCases.Produtos
{
    public class ProdutoUseCase : IProdutoUseCase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoUseCase(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutos()
        {
            return await _produtoRepository.ObterTodosProdutos();
        }

        public async Task<Produto?> ObterProdutoPorId(int id)
        {
            return await _produtoRepository.ObterProdutoPorId(id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorCategoria(string categoria)
        {
            return await _produtoRepository.ObterProdutosPorCategoria(categoria);
        }

        public async Task<Produto> CriarProduto(Produto produto)
        {
            await _produtoRepository.AdicionarProduto(produto);
            return produto;
        }

        public async Task AlterarProduto(int id, Produto produto)
        {
            var produtoExistente = await _produtoRepository.ObterProdutoPorId(id);
            if (produtoExistente == null)
            {
                throw new Exception($"Produto com id {id} não encontrado");
            }

            produto.Id = id;
            await _produtoRepository.AtualizarProduto(produto);
        }

        public async Task ExcluirProduto(int id)
        {
            await _produtoRepository.DeletarProduto(id);
        }
    }
}
