namespace SurveyBackend.Domain.Entities.Dto;

public class QuestionDto
{
    public int question_id { get; set; }
    
    public string question_text { get; set; } = string.Empty;
    
    public bool is_required { get; set; }
    
    public int question_order { get; set; }
    
    public string question_type { get; set; } = string.Empty;
    
    public List<string> options { get; set; } = [];
}
