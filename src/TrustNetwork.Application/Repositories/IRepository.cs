using System.Linq.Expressions;

namespace TrustNetwork.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdOrDefaultAsync(int id);
        void Add(T entity);
        void Update(T entity);
        Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate);

        Task SaveChangesAsync();
    }
}
