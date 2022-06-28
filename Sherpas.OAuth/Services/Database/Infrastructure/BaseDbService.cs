using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sherpas.OAuth.Data.Context;

namespace Sherpas.OAuth.Services.Database.Infrastructure
{
    public class BaseDbService<T> : IDbService<T> where T : class 
    {
        protected readonly AuthDbContext _context;

        protected DbSet<T> Entities => _context.Set<T>();

        public BaseDbService(AuthDbContext authDbContext)
        {
            _context = authDbContext;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await Entities.Where(expression).ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Entities.ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await Entities.AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
