using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loja.Models;
using Loja.DTO;
using Loja.Infra.Data;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly LojaDbContext _context;

    public UsuarioController(LojaDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
            u.Email == loginDto.Email && u.Senha == loginDto.Senha && u.Ativo);

        if (usuario == null)
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
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

        return Ok(new { message = "Login realizado com sucesso", usuario = usuarioDto });
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(Usuario usuario)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
        {
            return BadRequest(new { message = "Email já cadastrado" });
        }

        usuario.DataCadastro = DateTime.Now;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Usuário cadastrado com sucesso", usuarioId = usuario.Id });
    }

    [HttpGet("todos")]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
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
    public async Task<IActionResult> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

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