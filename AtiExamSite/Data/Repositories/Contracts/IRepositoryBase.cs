using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task AddRangeAsync(IEnumerable<Question> questions);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> SaveChangesAsync();
    }
}