using Loja.DTO;
using Loja.Infra.Usuarios;
using Loja.Models;
using Loja.UseCases.Base;

namespace Loja.UseCases.Usuarios;

public class UsuarioUseCase : BaseUseCase, IUsuarioUseCase
{
    private readonly IUsuarioDAO _usuarioDAO;

    public UsuarioUseCase(IUsuarioDAO usuarioDAO)
    {
        _usuarioDAO = usuarioDAO;
    }

    #region Todos_Usuarios

    public async Task<ResultadoUnico<UsuarioDTO>> LogarAsync(LoginDTO login)
    {
        try
        {
            var usuario = await _usuarioDAO.RetornarPorEmailAsync(login.Email);

            if (usuario == null || usuario.Senha != login.Senha || !usuario.Ativo)
                return FalhaObjeto<UsuarioDTO>([new("Email ou senha inválidos!")]);

            var usuarioDto = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                DataCadastro = usuario.DataCadastro,
                Endereco = usuario.Endereco,
                Cidade = usuario.Cidade,
                CEP = usuario.CEP,
                Estado = usuario.Estado
            };

            return SucessoObjeto(usuarioDto);
        }
        catch
        {
            return FalhaObjeto<UsuarioDTO>([new("Erro no login.", MensagemRetorno.EOrigem.Erro)]);
        }
    }

    public async Task<ResultadoUnico<UsuarioDTO>> RegistrarUsuario(UsuarioDTO usuarioDto)
    {
        try
        {
            if (await _usuarioDAO.EmailJaExisteAsync(usuarioDto.Email))
            {
                return FalhaObjeto<UsuarioDTO>([new("Email já cadastrado")]);
            }

            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                Senha = "123456", // Temporário - depois implementar hash
                Telefone = usuarioDto.Telefone,
                DataCadastro = DateTime.Now,
                Ativo = true,
                Endereco = usuarioDto.Endereco,
                Cidade = usuarioDto.Cidade,
                CEP = usuarioDto.CEP,
                Estado = usuarioDto.Estado
            };

            await _usuarioDAO.InserirAsync(usuario);

            usuarioDto.Id = usuario.Id;
            usuarioDto.DataCadastro = usuario.DataCadastro;

            return SucessoObjeto(usuarioDto);
        }
        catch
        {
            return FalhaObjeto<UsuarioDTO>([new("Erro na tentativa de registrar usuário.", MensagemRetorno.EOrigem.Erro)]);
        }
    }

    public async Task<ResultadoLista<UsuarioDTO>> ObterTodosUsuarios()
    {
        try
        {
            var usuarios = await _usuarioDAO.RetornarTodosAsync();
            var usuariosDto = usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Telefone = u.Telefone,
                DataCadastro = u.DataCadastro,
                Endereco = u.Endereco,
                Cidade = u.Cidade,
                CEP = u.CEP,
                Estado = u.Estado
            });

            return SucessoLista(usuariosDto);
        }
        catch
        {
            return FalhaLista<UsuarioDTO>([new("Erro na tentativa de obter usuários.", MensagemRetorno.EOrigem.Erro)]);
        }
    }

    #endregion

    #region Apenas_Logados

    public async Task<ResultadoUnico<UsuarioDTO>> ObterUsuarioPorId(int id)
    {
        if (idUsuarioLogado != id)
            return FalhaObjeto<UsuarioDTO>([new("Acesso não permitido.")]);

        try
        {
            var usuario = await _usuarioDAO.RetornarPorIdAsync(id);

            if (usuario == null)
                return FalhaObjeto<UsuarioDTO>([new("Usuário não encontrado.")]);

            var usuarioDto = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                DataCadastro = usuario.DataCadastro,
                Endereco = usuario.Endereco,
                Cidade = usuario.Cidade,
                CEP = usuario.CEP,
                Estado = usuario.Estado
            };

            return SucessoObjeto(usuarioDto);
        }
        catch
        {
            return FalhaObjeto<UsuarioDTO>([new("Erro na tentativa de obter usuário.", MensagemRetorno.EOrigem.Erro)]);
        }
    }

    public async Task<ResultadoVoid> AlterarUsuario(UsuarioDTO usuarioDto)
    {
        if (idUsuarioLogado != usuarioDto.Id)
            return Falha([new("Acesso não permitido.")]);

        try
        {
            var usuario = await _usuarioDAO.RetornarPorIdAsync(usuarioDto.Id);

            if (usuario == null)
                return Falha([new("Usuário não encontrado.")]);

            // Atualiza os dados
            usuario.Nome = usuarioDto.Nome;
            usuario.Email = usuarioDto.Email;
            usuario.Telefone = usuarioDto.Telefone;
            usuario.Endereco = usuarioDto.Endereco;
            usuario.Cidade = usuarioDto.Cidade;
            usuario.CEP = usuarioDto.CEP;
            usuario.Estado = usuarioDto.Estado;

            await _usuarioDAO.AlterarAsync(usuario);

            return Sucesso();
        }
        catch
        {
            return Falha([new("Erro na tentativa de alterar usuário.", MensagemRetorno.EOrigem.Erro)]);
        }
    }

    #endregion
}