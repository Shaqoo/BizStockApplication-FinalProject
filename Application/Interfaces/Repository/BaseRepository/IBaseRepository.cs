using Application.Pagination;
using System.Linq.Expressions;

namespace Application.Interfaces.Repository.BaseRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<PaginatedList<T>> GetAllAsync(PageRequest pageRequest);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByExpression(Expression<Func<T, bool>> predicate);
    }
}
