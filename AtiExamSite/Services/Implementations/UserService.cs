using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using AtiExamSite.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace AtiExamSite.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        #region [- Ctor -]
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        #endregion

        #region [- CreateAsync -]
        public async Task<bool> CreateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Check username uniqueness
            if (await _userRepository.ExistsByUsernameAsync(user.Username))
                throw new InvalidOperationException($"Username '{user.Username}' is already taken.");

            // Check email uniqueness
            if (await _userRepository.ExistsByEmailAsync(user.Email))
                throw new InvalidOperationException($"Email '{user.Email}' is already registered.");

            await _userRepository.AddAsync(user);
            return await _userRepository.SaveChangesAsync();
        }
        #endregion

        #region [- UpdateAsync -]
        public async Task<bool> UpdateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Check username uniqueness excluding current user
            if (await _userRepository.ExistsAsync(u => u.Username == user.Username && u.Id != user.Id))
                throw new InvalidOperationException($"Username '{user.Username}' is already taken by another user.");

            // Check email uniqueness excluding current user
            if (await _userRepository.ExistsAsync(u => u.Email == user.Email && u.Id != user.Id))
                throw new InvalidOperationException($"Email '{user.Email}' is already registered by another user.");

            await _userRepository.UpdateAsync(user);
            return await _userRepository.SaveChangesAsync();
        }
        #endregion

        #region [- GetByIdAsync -]
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        #endregion

        #region [- GetByUsernameAsync -]
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }
        #endregion

        #region [- DeleteAsync -]
        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return await _userRepository.SaveChangesAsync();
        }
        #endregion
    }
}
