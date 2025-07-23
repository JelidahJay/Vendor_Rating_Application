namespace SurveyBackend.Domain.Entities.Dto;

public class PendingSurveyDto
{
    public int survey_id { get; set; }
    public string token { get; set; } = string.Empty;
    public string rater_name { get; set; } = string.Empty;
    public string rater_email { get; set; } = string.Empty;
    public string vendor_name { get; set; } = string.Empty;
    public DateTime valid_until { get; set; }
}