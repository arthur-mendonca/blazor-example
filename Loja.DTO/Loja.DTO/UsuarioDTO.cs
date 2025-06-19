using System;

namespace Loja.DTO;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Telefone { get; set; }
    public DateTime DataCadastro { get; set; }
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? CEP { get; set; }
    public string? Estado { get; set; }
}
