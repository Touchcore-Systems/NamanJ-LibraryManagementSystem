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

        LogRecordHelper logRecord = new();
        public UsersController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        // POST: api/Users
        [HttpPost, Route("register")]
        public async Task<IActionResult> PostUsers(UserDTO userDto)
        {
            try
            {
                var userExists = _context.Users.Any(x => x.UName == userDto.UName);

                if (userExists)
                {
                    return new JsonResult("User already exists");
                }

                var users = new Users
                {
                    UName = userDto.UName,
                    UPass = HashPassHelper.hashPass(userDto.UPass),
                    URole = userDto.URole
                };

                if (users.URole == "admin" || users.URole == "student")
                {
                    await _userRepository.AddStudent(users);
                    return new JsonResult("User added");
                }
                else return new JsonResult("Role must be Admin or Student");
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }
    }
}
