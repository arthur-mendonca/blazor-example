using Loja.Infra.Base;
using Loja.Infra.Data;
using Loja.Models;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infra.Pedidos;

public class PedidoDAO : BaseDAO<Pedido>, IPedidoDAO
{
    public PedidoDAO(LojaDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pedido>> RetornarPedidosPorUsuarioAsync(int usuarioId)
    {
        return await SelecionarAsync(query =>
            query.Where(p => p.UsuarioId == usuarioId)
                 .Include(p => p.Itens)
                 .ThenInclude(i => i.Produto)
                 .OrderByDescending(p => p.DataPedido));
    }

    public async Task<IEnumerable<Pedido>> RetornarPedidosPorStatusAsync(StatusPedido status)
    {
        return await SelecionarAsync(query =>
            query.Where(p => p.Status == status)
                 .Include(p => p.Usuario)
                 .Include(p => p.Itens)
                 .ThenInclude(i => i.Produto)
                 .OrderByDescending(p => p.DataPedido));
    }

    public async Task<Pedido?> RetornarPedidoComItensAsync(int pedidoId)
    {
        return await SelecionarUnicoAsync(query =>
            query.Where(p => p.Id == pedidoId)
                 .Include(p => p.Usuario)
                 .Include(p => p.Itens)
                 .ThenInclude(i => i.Produto));
    }

    public async Task<IEnumerable<Pedido>> RetornarPedidosRecentesAsync(int dias = 30)
    {
        var dataLimite = DateTime.Now.AddDays(-dias);

        return await SelecionarAsync(query =>
            query.Where(p => p.DataPedido >= dataLimite)
                 .Include(p => p.Usuario)
                 .Include(p => p.Itens)
                 .ThenInclude(i => i.Produto)
                 .OrderByDescending(p => p.DataPedido));
    }

    public async Task<decimal> CalcularTotalVendasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Where(p => p.DataPedido >= dataInicio && p.DataPedido <= dataFim
                       && p.Status != StatusPedido.Cancelado)
            .SumAsync(p => p.ValorTotal);
    }

    public async Task AtualizarStatusAsync(int pedidoId, StatusPedido novoStatus)
    {
        var pedido = await _dbSet.FindAsync(pedidoId);
        if (pedido != null)
        {
            pedido.Status = novoStatus;
            await _context.SaveChangesAsync();
        }
    }
}