using Microsoft.EntityFrameworkCore;
using SurveyBanckend.Infrastructure;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBackend.Domain.Entities.Dto;

namespace SurveyBackend.Api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly DataContext _context;

        public DepartmentService(DataContext context)
        {
            _context = context;
        }

        public async Task<Department> CreateDepartmentAsync(DepartmentCreateDTO dto)
        {
            var department = new Department
            {
                name = dto.name
            };

            _context.departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            var departments = await _context.departments
                .Include(d => d.users)
                .ToListAsync();

            return departments.Select(d => new DepartmentDto
            {
                department_id = d.department_id,
                name = d.name,
                users = d.users.Select(u => new UserDto
                {
                    user_id = u.user_id,
                    full_name = u.full_name,
                    email = u.email,
                    role = u.role
                }).ToList()
            }).ToList();
        }
        
        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            var department = await _context.departments
                .Include(d => d.users)
                .FirstOrDefaultAsync(d => d.department_id == id);

            if (department == null)
                return null;

            return new DepartmentDto
            {
                department_id = department.department_id,
                name = department.name,
                users = department.users.Select(u => new UserDto
                {
                    user_id = u.user_id,
                    full_name = u.full_name,
                    email = u.email,
                    role = u.role
                }).ToList()
            };
        }



        public async Task<Department?> UpdateDepartmentAsync(int id, DepartmentCreateDTO dto)
        {
            var department = await _context.departments.FindAsync(id);
            if (department == null) return null;

            // Check if name already exists
            var existing = await _context.departments
                .FirstOrDefaultAsync(d => d.department_id != id && d.name.ToLower() == dto.name.ToLower());

            if (existing != null)
                throw new InvalidOperationException("Another department with the same name already exists.");

            department.name = dto.name;

            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.departments.FindAsync(id);
            if (department == null) return false;

            _context.departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}