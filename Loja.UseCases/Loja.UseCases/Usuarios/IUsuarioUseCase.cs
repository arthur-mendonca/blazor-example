using Loja.DTO;
using Loja.UseCases.Base;

namespace Loja.UseCases.Usuarios;

public interface IUsuarioUseCase
{
    void IdentificarAcesso(long idUsuario);

    #region Todos_Usuarios

    Task<ResultadoUnico<UsuarioDTO>> LogarAsync(LoginDTO login);
    Task<ResultadoUnico<UsuarioDTO>> RegistrarUsuario(UsuarioDTO usuario);
    Task<ResultadoLista<UsuarioDTO>> ObterTodosUsuarios();

    #endregion

    #region Apenas_Logados

    Task<ResultadoUnico<UsuarioDTO>> ObterUsuarioPorId(int id);
    Task<ResultadoVoid> AlterarUsuario(UsuarioDTO usuario);

    #endregion
}