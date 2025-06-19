using Loja.Infra.Data;
using Loja.Models;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infra.Base;

public abstract class BaseDAO<T> : IBaseDAO<T> where T : class, IModel
{
    protected readonly LojaDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseDAO(LojaDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task InserirAsync(T obj)
    {
        await _dbSet.AddAsync(obj);
        await _context.SaveChangesAsync();
    }

    public virtual async Task AlterarAsync(T obj)
    {
        _dbSet.Update(obj);
        await _context.SaveChangesAsync();
    }

    public virtual async Task ExcluirAsync(int id)
    {
        var obj = await RetornarPorIdAsync(id);
        if (obj != null)
        {
            _dbSet.Remove(obj);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<T?> RetornarPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> RetornarTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    protected async Task<IEnumerable<T>> SelecionarAsync(Func<IQueryable<T>, IQueryable<T>> query)
    {
        return await query(_dbSet).ToListAsync();
    }

    protected async Task<T?> SelecionarUnicoAsync(Func<IQueryable<T>, IQueryable<T>> query)
    {
        return await query(_dbSet).FirstOrDefaultAsync();
    }
}