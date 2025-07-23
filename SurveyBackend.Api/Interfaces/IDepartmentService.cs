using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBackend.Domain.Entities.Dto;

namespace SurveyBackend.Api.Interfaces
{
    public interface IDepartmentService
    {
        Task<Department> CreateDepartmentAsync(DepartmentCreateDTO dto);
        Task<List<DepartmentDto>> GetDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(int id);
        Task<Department?> UpdateDepartmentAsync(int id, DepartmentCreateDTO dto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}