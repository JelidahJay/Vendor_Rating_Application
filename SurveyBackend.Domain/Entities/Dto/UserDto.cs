namespace SurveyBackend.Domain.Entities.Dto;

public class UserDto
{
    public int user_id { get; set; }
    public string full_name { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
}

