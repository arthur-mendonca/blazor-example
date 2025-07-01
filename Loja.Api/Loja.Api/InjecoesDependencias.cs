using Loja.Infra.Data;
using Loja.Infra.Usuarios;
using Loja.Infra.Pedidos;
using Loja.Infra.Produtos;
using Loja.UseCases.Usuarios;
using Loja.UseCases.Produtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;


namespace Loja.Api
{
    public static class InjecoesDependencias
    {
        internal static void InjetarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            #region Entity Framework

            services.AddDbContext<LojaDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Loja.Api")));

            #endregion

            #region CORS

            //     services.AddCors(options =>
            //   {
            //       options.AddPolicy("AllowBlazorApp", policy =>
            //       {
            //           policy.WithOrigins(
            //           "https://localhost:7038",
            //           "http://localhost:5185",
            //           "https://localhost:5185") // URLs do Blazor
            //                 .AllowAnyHeader()
            //                 .AllowAnyMethod()
            //                 .AllowCredentials();
            //       });
            //   });

            #endregion


            #region Authentication & Authorization

            services.AddScoped<TokenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config.Instancia.ChavePrivada)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            #endregion


            #region DAOs

            services.AddScoped<IUsuarioDAO, UsuarioDAO>();
            services.AddScoped<IPedidoDAO, PedidoDAO>();
            services.AddScoped<IItemPedidoDAO, ItemPedidoDAO>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            #endregion

            #region UseCases

            services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
            services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
            // services.AddScoped<IPedidoUseCase, PedidoUseCase>(); // Quando criar o PedidoUseCase

            #endregion

            #region Mappers

            // Quando implementar mappers:
            // services.AddScoped<IMapper<Usuario, UsuarioDTO>, UsuarioMapper>();
            // services.AddScoped<IMapper<Pedido, PedidoDTO>, PedidoMapper>();

            #endregion

            #region Authentication

            // Quando implementar autenticação:
            // services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

            #endregion
        }
    }
}