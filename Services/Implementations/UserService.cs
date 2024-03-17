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

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUser()
        {
            var userWithMostOrders = await _dbContext.Users
                .Select(u => new User
                    {
                    Id = u.Id,
                    Email = u.Email,
                    Orders = u.Orders,
                })
                .OrderByDescending(u => u.Orders.Count)
                .FirstOrDefaultAsync();

            if (userWithMostOrders == null)
                return new User();

            return userWithMostOrders;

        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.Where(u => u.Status == Enums.UserStatus.Inactive).ToListAsync(); ;
        }
    }
}
