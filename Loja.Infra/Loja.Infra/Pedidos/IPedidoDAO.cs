using Loja.Infra.Base;
using Loja.Models;

namespace Loja.Infra.Pedidos;

public interface IPedidoDAO : IBaseDAO<Pedido>
{
    Task<IEnumerable<Pedido>> RetornarPedidosPorUsuarioAsync(int usuarioId);
    Task<IEnumerable<Pedido>> RetornarPedidosPorStatusAsync(StatusPedido status);
    Task<Pedido?> RetornarPedidoComItensAsync(int pedidoId);
    Task<IEnumerable<Pedido>> RetornarPedidosRecentesAsync(int dias = 30);
    Task<decimal> CalcularTotalVendasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task AtualizarStatusAsync(int pedidoId, StatusPedido novoStatus);
}