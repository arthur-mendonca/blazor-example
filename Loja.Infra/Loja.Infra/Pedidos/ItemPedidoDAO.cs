using Loja.Infra.Base;
using Loja.Infra.Data;
using Loja.Models;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infra.Pedidos;

public class ItemPedidoDAO : BaseDAO<ItemPedido>, IItemPedidoDAO
{
    public ItemPedidoDAO(LojaDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ItemPedido>> RetornarItensPorPedidoAsync(int pedidoId)
    {
        return await SelecionarAsync(query =>
            query.Where(i => i.PedidoId == pedidoId)
                 .Include(i => i.Produto));
    }

    public async Task<IEnumerable<ItemPedido>> RetornarItensPorProdutoAsync(int produtoId)
    {
        return await SelecionarAsync(query =>
            query.Where(i => i.ProdutoId == produtoId)
                 .Include(i => i.Pedido)
                 .ThenInclude(p => p.Usuario));
    }

    public async Task<int> ContarQuantidadeVendidaPorProdutoAsync(int produtoId, DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Where(i => i.ProdutoId == produtoId
                       && i.Pedido.DataPedido >= dataInicio
                       && i.Pedido.DataPedido <= dataFim
                       && i.Pedido.Status != StatusPedido.Cancelado)
            .SumAsync(i => i.Quantidade);
    }

    public async Task RemoverTodosItensDoPedidoAsync(int pedidoId)
    {
        var itens = await _dbSet.Where(i => i.PedidoId == pedidoId).ToListAsync();
        _dbSet.RemoveRange(itens);
        await _context.SaveChangesAsync();
    }
}