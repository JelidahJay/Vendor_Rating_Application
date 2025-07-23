namespace SurveyBackend.Domain.Entities.Dto;

public class SurveyAssignDto
{
    public int vendor_id { get; set; }
    public int user_id { get; set; } // Rater
    public int invited_by_user_id { get; set; } // Admin or Manager
    public int valid_days { get; set; } = 7; // default validity in days
}