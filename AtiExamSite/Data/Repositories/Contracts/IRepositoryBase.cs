using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<List<T>> GetByIdsAsync(IEnumerable<string> ids);
        Task AddRangeAsync(IEnumerable<Question> questions);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> SaveChangesAsync();
    }
}