using Microsoft.EntityFrameworkCore;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBanckend.Infrastructure;

namespace SurveyBackend.Api.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(UserCreateDTO dto)
        {
            var user = new User
            {
                full_name = dto.full_name,
                email = dto.email,
                role = dto.role,
                department_id = dto.department_id,
                password = dto.role == "admin" && !string.IsNullOrWhiteSpace(dto.password)
                    ? BCrypt.Net.BCrypt.HashPassword(dto.password)
                    : null
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.users.Include(u => u.department).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.users.Include(u => u.department).FirstOrDefaultAsync(u => u.user_id == id);
        }

        public async Task<User?> UpdateUserAsync(int id, UserCreateDTO dto)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null) return null;

            user.full_name = dto.full_name;
            user.email = dto.email;
            user.role = dto.role;
            user.department_id = dto.department_id;

            if (dto.role == "admin" && !string.IsNullOrWhiteSpace(dto.password))
            {
                user.password = BCrypt.Net.BCrypt.HashPassword(dto.password);
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null) return false;

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
