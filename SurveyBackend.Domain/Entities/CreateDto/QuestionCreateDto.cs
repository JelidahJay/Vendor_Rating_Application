namespace SurveyBackend.Domain.Entities.CreateDto;

using System.ComponentModel.DataAnnotations;

public class QuestionCreateDto
{
    [Required]
    public string question_text { get; set; } = string.Empty;

    [Required]
    public bool is_required { get; set; }

    [Range(1, int.MaxValue)]
    public int question_order { get; set; }

    [Range(1, int.MaxValue)]
    public int question_type_id { get; set; }

    public List<string>? options { get; set; }
}