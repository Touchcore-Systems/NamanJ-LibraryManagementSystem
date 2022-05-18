using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        LogRecord logRecord = new();
        public UsersController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        // POST: api/Users
        [HttpPost, Route("register")]
        public async Task<ActionResult<Users>> PostUsers(UserDTO userDto)
        {
            var users = new Users
            {
                UName = userDto.UName,
                UPass = HashPass.hashPass(userDto.UPass),
                URole = userDto.URole
            };

            string res = String.Empty;
            try
            {
                if (users.URole == "admin" || users.URole == "student")
                {
                    await _userRepository.AddStudent(users);
                    res = "User added";
                }
                else res = "Role must be Admin or Student";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                logRecord.LogWriter(res);
            }
            return new JsonResult(res);
        }
    }
}
