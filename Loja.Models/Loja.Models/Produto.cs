namespace Loja.Models;

public class Produto : IModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public decimal Preco { get; set; }
    public string Imagem { get; set; } = default!;
    public string Categoria { get; set; } = default!;
    public string? Descricao { get; set; }
    public int EstoqueQuantidade { get; set; }
    public bool Ativo { get; set; } = true;
}