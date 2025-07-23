using System.ComponentModel.DataAnnotations;

namespace SurveyBackend.Domain.Entities
{
    public class QuestionOption
    {
        [Key]
        public int question_option_id { get; set; } // new primary key

        public int question_id { get; set; }
        public Questions? question { get; set; }

        public int option_id { get; set; }
        public Option? option { get; set; } // navigates to Option entity
    }

}