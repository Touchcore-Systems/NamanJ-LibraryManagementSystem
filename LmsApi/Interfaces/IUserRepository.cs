using LmsApi.Models;

namespace LmsApi.Interfaces
{
    public interface IUserRepository
    {
        Task<Users> AddStudent(Users users);
    }
}
