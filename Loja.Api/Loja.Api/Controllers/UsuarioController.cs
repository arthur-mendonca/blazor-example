using Microsoft.AspNetCore.Mvc;
using Loja.Models;
using Loja.DTO;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    // Lista temporária de usuários (depois substituir por banco de dados)
    private static List<Usuario> usuarios = new List<Usuario>
    {
        new Usuario { Id = 1, Nome = "Admin", Email = "admin@loja.com", Senha = "123456" }
    };

    [HttpPost("login")]
    public IActionResult Login(LoginDTO loginDto)
    {
        var usuario = usuarios.FirstOrDefault(u =>
            u.Email == loginDto.Email && u.Senha == loginDto.Senha && u.Ativo);

        if (usuario == null)
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }

        // Retorna dados do usuário sem a senha
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

        return Ok(new { message = "Login realizado com sucesso", usuario = usuarioDto });
    }

    [HttpPost("registro")]
    public IActionResult Registro(Usuario usuario)
    {
        // Verifica se email já existe
        if (usuarios.Any(u => u.Email == usuario.Email))
        {
            return BadRequest(new { message = "Email já cadastrado" });
        }

        // Gera novo ID
        usuario.Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1;
        usuario.DataCadastro = DateTime.Now;

        usuarios.Add(usuario);

        return Ok(new { message = "Usuário cadastrado com sucesso", usuarioId = usuario.Id });
    }


    [HttpGet("todos")]
    public IActionResult GetUsuarios()
    {
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
        }).ToList();

        return Ok(usuariosDto);
    }

    [HttpGet("{id}")]
    public IActionResult GetUsuario(int id)
    {
        var usuario = usuarios.FirstOrDefault(u => u.Id == id);

        if (usuario == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }

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

        return Ok(usuarioDto);
    }

}