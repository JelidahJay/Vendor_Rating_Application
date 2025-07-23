using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities
{
    public class QuestionType
    {
        [Key]
        public int id { get; set; }
        public string type_name { get; set; } = string.Empty;

        public ICollection<Questions>? questions { get; set; }
    }
}