namespace Loja.Models;

public class Pedido
{

    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;
    public DateTime DataPedido { get; set; } = DateTime.Now;
    public decimal ValorTotal { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Pendente;
    public string? EnderecoEntrega { get; set; }

    // Itens do pedido
    public List<ItemPedido> Itens { get; set; } = new();
}

public enum StatusPedido
{
    Pendente,
    Confirmado,
    Enviado,
    Entregue,
    Cancelado
}