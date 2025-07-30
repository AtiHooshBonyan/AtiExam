using AtiExamSite.Models.DomainModels;
using System;
using System.Threading.Tasks;

namespace AtiExamSite.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> DeleteAsync(Guid id);
    }
}
