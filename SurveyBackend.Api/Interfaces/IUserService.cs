using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;

namespace SurveyBackend.Api.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserCreateDTO dto);
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> UpdateUserAsync(int id, UserCreateDTO dto);
        Task<bool> DeleteUserAsync(int id);
    }
}