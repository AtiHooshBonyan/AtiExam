using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public async Task<T?> GetByIdAsync(string id)
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

        #region [- GetByIdsAsync() -]
        public async Task<List<T>> GetByIdsAsync(IEnumerable<string> ids)
        {
            // Assumes T has a property named "Id" of type Guid
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.PropertyOrField(parameter, "Id");
            if (property.Type != typeof(string))
                throw new InvalidOperationException("Entity does not have a Guid Id property.");

            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(string));

            var idsConstant = Expression.Constant(ids);
            var body = Expression.Call(containsMethod, idsConstant, property);
            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

            return await _dbContext.Set<T>().Where(lambda).ToListAsync();
        } 
        #endregion

        #region [- AddRangeAsync() -]
        public async Task AddRangeAsync(IEnumerable<Question> questions)
        {
            await _dbContext.Set<Question>().AddRangeAsync(questions);
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

    }
}