using System.Linq.Expressions;
namespace AtiExamSite.Data.Repositories.Contracts
{

    public interface IRepositoryBase<T> where T : class
    {
        #region [- Core CRUD Operations -]
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(Guid id);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        #endregion
    }
}