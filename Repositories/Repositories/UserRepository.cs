using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        #region CREATE
        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        #endregion

        #region READ
        public Task<User?> GetByUserNameAsync(string userName) => 
            _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);

        public Task<bool> UserNameExistsAsync(string userName) =>
            _context.Users.AnyAsync(u => u.UserName == userName);

        public Task<bool> EmailExistsAsync(string email) =>
            _context.Users.AnyAsync(u => u.Email == email);
        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        #endregion
    }
}
