namespace SurveyBackend.Domain.Entities.Dto;

public class SurveySummaryDto
{
    public string rater_name { get; set; } = string.Empty;
    public string rater_email { get; set; } = string.Empty;
    public string vendor_name { get; set; } = string.Empty;
    public DateTime? submitted_at { get; set; }
}