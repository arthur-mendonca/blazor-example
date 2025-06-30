using System.Diagnostics;
using System.Reflection;
using Loja.DTO;
using Loja.UseCases.Usuarios;

namespace Loja.Api.Endpoints
{
    public static class AdicionarEndpointsUsuariosExtensions
    {
        /// <summary>
        /// Método para adicionar os endpoints de usuários da API.
        /// </summary>
        /// <param name="app">Instância do WebApplication.</param>
        public static void AdicionarEndpointsUsuarios(this WebApplication app)
        {
            var usuarios = app.MapGroup("/api/usuarios")
                .WithTags("Usuários")
                .WithDescription("Endpoints relacionados a usuários");

            usuarios.MapPost("/login", Login)
                .WithName("Login")
                .WithSummary("Realiza o login do usuário no sistema");

            usuarios.MapPost("/registro", RegistrarUsuario)
                .WithName("Registrar usuário")
                .WithSummary("Registra um novo usuário no sistema");

            usuarios.MapGet("/todos", ObterTodosUsuarios)
                .WithName("Obter todos os usuários")
                .WithSummary("Retorna todos os usuários do sistema");

            usuarios.MapGet("/{id}", ObterUsuarioPorId)
                .WithName("Obter usuário por ID")
                .WithSummary("Retorna um usuário específico pelo ID");

            usuarios.MapPut("/{id}", AlterarUsuario)
                .WithName("Alterar usuário")
                .WithSummary("Altera os dados de um usuário")
                .RequireAuthorization();
        }

        private static async Task<IResult> Login(LoginDTO loginDto, IUsuarioUseCase usuarioUseCase, TokenService tokenService)
        {
            try
            {
                var resultado = await usuarioUseCase.LogarAsync(loginDto);

                if (!resultado.Sucesso)
                    return TypedResults.Unauthorized();

                var token = tokenService.Gerar(resultado.Objeto!);

                return TypedResults.Ok(new
                {
                    message = "Login realizado com sucesso",
                    usuario = resultado.Objeto,
                    token = token
                });
            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> RegistrarUsuario(UsuarioDTO usuarioDto, IUsuarioUseCase usuarioUseCase)
        {
            try
            {
                var resultado = await usuarioUseCase.RegistrarUsuario(usuarioDto);

                return resultado.Sucesso
                    ? TypedResults.Created($"/api/usuarios/{resultado.Objeto?.Id}", new { message = "Usuário registrado com sucesso", usuario = resultado.Objeto })
                    : TypedResults.BadRequest(new { message = resultado.Mensagens.First().Texto });
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> ObterTodosUsuarios(IUsuarioUseCase usuarioUseCase)
        {
            try
            {
                var resultado = await usuarioUseCase.ObterTodosUsuarios();

                return resultado.Sucesso
                    ? TypedResults.Ok(resultado.Objetos)
                    : TypedResults.BadRequest(new { message = resultado.Mensagens.First().Texto });
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> ObterUsuarioPorId(int id, IUsuarioUseCase usuarioUseCase)
        {
            try
            {
                // Simular usuário logado - depois implementar autenticação real
                usuarioUseCase.IdentificarAcesso(id);

                var resultado = await usuarioUseCase.ObterUsuarioPorId(id);

                return resultado.Sucesso
                    ? TypedResults.Ok(resultado.Objeto)
                    : TypedResults.NotFound(new { message = resultado.Mensagens.First().Texto });
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }

        private static async Task<IResult> AlterarUsuario(int id, UsuarioDTO usuarioDto, IUsuarioUseCase usuarioUseCase)
        {
            try
            {
                usuarioUseCase.IdentificarAcesso(id);

                if (id != usuarioDto.Id)
                    return TypedResults.BadRequest(new { message = "O ID não confere." });

                var resultado = await usuarioUseCase.AlterarUsuario(usuarioDto);

                return resultado.Sucesso
                    ? TypedResults.Ok(new { message = "Usuário alterado com sucesso" })
                    : TypedResults.BadRequest(new { message = resultado.Mensagens.First().Texto });
            }
            catch (Exception ex)
            {
#if DEBUG
                var metodo = MethodBase.GetCurrentMethod();
                if (metodo != null)
                    Debug.WriteLine($"Exception in {metodo.Name}: {ex.Message}");
#endif
                return TypedResults.InternalServerError();
            }
        }
    }
}