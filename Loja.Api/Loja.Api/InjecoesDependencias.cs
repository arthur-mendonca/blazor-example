using Loja.Infra.Data;
using Loja.Infra.Usuarios;
using Loja.Infra.Pedidos;
using Loja.UseCases.Usuarios;
using Microsoft.EntityFrameworkCore;

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

            #region DAOs

            services.AddScoped<IUsuarioDAO, UsuarioDAO>();
            services.AddScoped<IPedidoDAO, PedidoDAO>();
            services.AddScoped<IItemPedidoDAO, ItemPedidoDAO>();

            #endregion

            #region UseCases

            services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
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