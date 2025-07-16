using AtiExamSite.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AtiExamSite.Data.Repositories.RepositoriesImplementation
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ProjectDbContext _dbContext;

        #region [- Ctor() -]
        public BaseRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region [- FindAsync() -]
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region [- GetByIdAsync() -]
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region [- AddAsync() -]
        public async Task<bool> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task<bool> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- ExistsAsync() -]
        public async Task<bool> ExistsAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }
        #endregion
    }
}