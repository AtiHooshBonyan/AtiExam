using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> SaveChangesAsync();
    }
}