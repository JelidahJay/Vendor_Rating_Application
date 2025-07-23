namespace SurveyBackend.Domain.Entities.Dto;

public class SurveyFillDto
{
    public string rater_name { get; set; } = string.Empty;
    public string rater_email { get; set; } = string.Empty;
    public string vendor_name { get; set; } = string.Empty;
    public List<QuestionDto> survey_questions { get; set; } = new();
}