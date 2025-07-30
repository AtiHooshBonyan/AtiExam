using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ProjectDbContext _dbContext;

        #region [- Ctor() -]
        protected RepositoryBase(ProjectDbContext context)
        {
            _dbContext = context;
        }
        #endregion

        #region [- GetByIdAsync() -]
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        #endregion

        #region [- AddAsync() -]
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await Task.CompletedTask;
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }
        #endregion

        #region [- SaveChangesAsync() -]
        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- ExistsAsync() -]
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Set<T>().AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }
        #endregion
    }
}