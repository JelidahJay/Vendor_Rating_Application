using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities;

public class Option
{
    [Key]
    public int option_id { get; set; }

    public string option_text { get; set; } = string.Empty;

    public ICollection<QuestionOption>? question_options { get; set; }
}
