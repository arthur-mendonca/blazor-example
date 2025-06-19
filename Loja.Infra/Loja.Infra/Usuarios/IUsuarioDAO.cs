using Loja.Infra.Base;
using Loja.Models;

namespace Loja.Infra.Usuarios;

public interface IUsuarioDAO : IBaseDAO<Usuario>
{
    Task<Usuario?> RetornarPorEmailAsync(string email);
    Task<bool> EmailJaExisteAsync(string email, int idUsuarioAtual = 0);
    Task<IEnumerable<Usuario>> RetornarUsuariosAtivosAsync();
}