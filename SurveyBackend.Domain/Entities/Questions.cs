using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities;

/**
 * Entity for the Questions for all the surveys
 */
public class Questions
{
    [Key]
    public int question_id { get; set; }
    
    public string question_text { get; set; } = string.Empty;
    
    public bool is_required { get; set; }
    
    public int question_order { get; set; }

    public int question_type_id { get; set; }
    
    public QuestionType? question_type { get; set; }

    public ICollection<QuestionOption>? options { get; set; }
}