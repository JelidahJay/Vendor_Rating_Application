using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities;

public class SurveyResponse
{
    [Key]
    public int response_id { get; set; }

    public int survey_id { get; set; }
    public Survey? survey { get; set; }

    public int question_id { get; set; }
    public Questions? question { get; set; }

    public string answer { get; set; } = string.Empty;
}