using LmsApi.Data;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using LmsApi.Models;

namespace LmsApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        LogRecordHelper logRecord = new();
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
                logRecord.LogWriter("User added");
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
            }
            return users;
        }
    }
}
