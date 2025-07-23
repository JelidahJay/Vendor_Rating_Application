using Microsoft.AspNetCore.Mvc;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities.CreateDto;

namespace SurveyBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO dto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.user_id }, user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating user: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userService.GetUsersAsync();

            var flattenedUsers = users.Select(u => new
            {
                user_id = u.user_id,
                full_name = u.full_name,
                email = u.email,
                role = u.role,
                department_id = u.department_id,
                department = u.department == null ? null : new
                {
                    department_id = u.department.department_id,
                    name = u.department.name
                }
            });

            return Ok(flattenedUsers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving users: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving user: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserCreateDTO dto)
    {
        try
        {
            var updated = await _userService.UpdateUserAsync(id, dto);
            if (updated == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(updated);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating user: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound($"User with ID {id} not found.");

            return Ok(new { message = "User deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting user: {ex.Message}");
        }
    }
}
