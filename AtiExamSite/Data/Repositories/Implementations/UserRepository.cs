using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        #region [- Ctor() -]
        public UserRepository(ProjectDbContext context) : base(context)
        {

        }
        #endregion

        #region [- GetByUsernameAsync() -]
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        #endregion

        #region [- GetByEmailAsync() -]
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        #endregion

        #region [- ExistsByUsernameAsync() -]
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }
        #endregion

        #region [- ExistsByEmailAsync() -]
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }
        #endregion

        #region [- ExistsAsync() -]
        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.AnyAsync(predicate);
        }
        #endregion

        #region [-UpdateAsync() -]
        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await Task.CompletedTask;
        }
        #endregion

        #region [- SaveChangesAsync() -]
        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync()) > 0;
        } 
        #endregion
    }
}
