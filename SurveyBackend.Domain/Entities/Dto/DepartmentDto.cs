namespace SurveyBackend.Domain.Entities.Dto;

public class DepartmentDto
{
    public int department_id { get; set; }
    public string name { get; set; } = string.Empty;
    public List<UserDto> users { get; set; } = new();
    
}