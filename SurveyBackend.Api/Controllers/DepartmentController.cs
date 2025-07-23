namespace SurveyBackend.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities.CreateDto;


[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateDTO dto)
    {
        try
        {
            var department = await _departmentService.CreateDepartmentAsync(dto);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.department_id }, department);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating department: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        try
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            return Ok(departments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving departments: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        try
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
                return NotFound($"Department with ID {id} not found.");

            return Ok(department);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving department: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentCreateDTO dto)
    {
        try
        {
            var updated = await _departmentService.UpdateDepartmentAsync(id, dto);
            if (updated == null)
                return NotFound($"Department with ID {id} not found.");

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating department: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        try
        {
            var deleted = await _departmentService.DeleteDepartmentAsync(id);
            if (!deleted)
                return NotFound($"Department with ID {id} not found.");

            return Ok(new { message = "Department deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting department: {ex.Message}");
        }
    }
}
