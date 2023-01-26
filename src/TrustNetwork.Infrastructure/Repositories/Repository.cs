using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrustNetwork.Application.Repositories;

namespace TrustNetwork.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity) => _dbSet.Add(entity);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdOrDefaultAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Update(T entity) => _dbSet.Update(entity);
    }
}
