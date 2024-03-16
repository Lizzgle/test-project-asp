using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<User> _user;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _user = dbContext.Users;
        }

        public async Task<User> GetUser()
        {
            try
            {
                var userWithMostOrders = await _dbContext.Users
                    .Include(u => u.Orders)
                    .OrderByDescending(u => u.Orders.Count)
                    .FirstOrDefaultAsync();

                return userWithMostOrders;
            }
            catch (Exception)
            {
                return new User();
            }
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.Where(u => u.Status == Enums.UserStatus.Inactive).ToListAsync(); ;
        }
    }
}
