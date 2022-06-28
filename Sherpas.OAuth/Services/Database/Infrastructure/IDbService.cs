using System.Linq.Expressions;

namespace Sherpas.OAuth.Services.Database.Infrastructure
{
    public interface IDbService<T> where T : class
    {
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}