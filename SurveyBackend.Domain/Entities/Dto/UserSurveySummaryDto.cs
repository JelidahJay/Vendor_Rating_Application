namespace SurveyBackend.Domain.Entities.Dto;

public class UserSurveySummaryDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<SurveySummaryDto> Surveys { get; set; } = new();
}