using LmsApi.Data;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Users> AddStudent(Users users)
        {
            try
            {
                await _context.Users.AddAsync(users);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }
    }
}
