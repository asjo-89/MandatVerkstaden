using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<bool> UserNameExistsAsync(string userName);
        Task<User> AddUserAsync(User user);
    }
}
