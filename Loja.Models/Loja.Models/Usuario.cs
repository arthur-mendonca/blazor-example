namespace Loja.Models;

public class Usuario : IModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
    public string? Telefone { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public bool Ativo { get; set; } = true;

    // Endereço para entrega
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? CEP { get; set; }
    public string? Estado { get; set; }

    // Lista de pedidos do usuário
    public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
}