using Loja.Infra.Data;
using Loja.Infra.Produtos;
using Loja.Models;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infra.Produtos
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly LojaDbContext _context;

        public ProdutoRepository(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutos()
        {
            return await _context.Set<Produto>().AsNoTracking().ToListAsync();
        }

        public async Task<Produto?> ObterProdutoPorId(int id)
        {
            return await _context.Set<Produto>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorCategoria(string categoria)
        {
            return await _context.Set<Produto>().AsNoTracking().Where(p => p.Categoria == categoria).ToListAsync();
        }

        public async Task AdicionarProduto(Produto produto)
        {
            await _context.Set<Produto>().AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarProduto(Produto produto)
        {
            _context.Set<Produto>().Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarProduto(int id)
        {
            var produto = await ObterProdutoPorId(id);
            if (produto != null)
            {
                _context.Set<Produto>().Remove(produto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
