using AtiExamSite.Models.DomainModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);

        Task UpdateAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}
