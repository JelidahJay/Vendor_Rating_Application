namespace SurveyBackend.Domain.Entities.Dto;

public class MultiSurveyAssignDto
{
    public int vendor_id { get; set; }
    public List<int> user_ids { get; set; } = [];
    public int invited_by_user_id { get; set; }
    public int valid_days { get; set; } = 7;
}

