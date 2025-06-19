using Loja.Models;

namespace Loja.Infra.Base;

public interface IBaseDAO<T> where T : IModel
{
    Task InserirAsync(T obj);
    Task AlterarAsync(T obj);
    Task ExcluirAsync(int id);
    Task<T?> RetornarPorIdAsync(int id);
    Task<IEnumerable<T>> RetornarTodosAsync();
}