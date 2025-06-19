using Loja.Infra.Base;
using Loja.Infra.Data;
using Loja.Models;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infra.Usuarios;

public class UsuarioDAO : BaseDAO<Usuario>, IUsuarioDAO
{
    public UsuarioDAO(LojaDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> RetornarPorEmailAsync(string email)
    {
        return await SelecionarUnicoAsync(query =>
            query.Where(u => u.Email == email));
    }

    public async Task<bool> EmailJaExisteAsync(string email, int idUsuarioAtual = 0)
    {
        return await _dbSet.AnyAsync(u =>
            u.Email == email && u.Id != idUsuarioAtual);
    }

    public async Task<IEnumerable<Usuario>> RetornarUsuariosAtivosAsync()
    {
        return await SelecionarAsync(query =>
            query.Where(u => u.Ativo));
    }
}