namespace SurveyBackend.Domain.Entities.Dto;

public class DepartmentSurveySummaryDto
{
    public string department_name { get; set; } = string.Empty;
    public List<SurveySummaryDto> surveys { get; set; } = new();
}