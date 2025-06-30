using Microsoft.AspNetCore.Mvc;
using Loja.DTO;
using Loja.UseCases.Usuarios;
using Loja.Api;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioUseCase _usuarioUseCase;

    public UsuarioController(IUsuarioUseCase usuarioUseCase)
    {
        _usuarioUseCase = usuarioUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDto, TokenService tokenService)
    {
        var resultado = await _usuarioUseCase.LogarAsync(loginDto);

        if (!resultado.Sucesso)
            return Unauthorized(new { message = resultado.Mensagens.First().Texto });

        var token = tokenService.Gerar(resultado.Objeto!);

        return Ok(new
        {
            message = "Login realizado com sucesso",
            usuario = resultado.Objeto,
            token = token  // ADICIONE ESTA LINHA
        });
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(UsuarioDTO usuarioDto)
    {
        var resultado = await _usuarioUseCase.RegistrarUsuario(usuarioDto);

        if (!resultado.Sucesso)
            return BadRequest(new { message = resultado.Mensagens.First().Texto });

        return Ok(new { message = "Usuário registrado com sucesso", usuario = resultado.Objeto });
    }

    [HttpGet("todos")]
    public async Task<IActionResult> GetUsuarios()
    {
        var resultado = await _usuarioUseCase.ObterTodosUsuarios();

        if (!resultado.Sucesso)
            return BadRequest(new { message = resultado.Mensagens.First().Texto });

        return Ok(resultado.Objetos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuario(int id)
    {
        // Simular usuário logado - depois implementar autenticação real
        _usuarioUseCase.IdentificarAcesso(id);

        var resultado = await _usuarioUseCase.ObterUsuarioPorId(id);

        if (!resultado.Sucesso)
            return NotFound(new { message = resultado.Mensagens.First().Texto });

        return Ok(resultado.Objeto);
    }
}