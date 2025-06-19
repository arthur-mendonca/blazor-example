using Loja.Infra.Base;
using Loja.Models;

namespace Loja.Infra.Pedidos;

public interface IItemPedidoDAO : IBaseDAO<ItemPedido>
{
    Task<IEnumerable<ItemPedido>> RetornarItensPorPedidoAsync(int pedidoId);
    Task<IEnumerable<ItemPedido>> RetornarItensPorProdutoAsync(int produtoId);
    Task<int> ContarQuantidadeVendidaPorProdutoAsync(int produtoId, DateTime dataInicio, DateTime dataFim);
    Task RemoverTodosItensDoPedidoAsync(int pedidoId);
}