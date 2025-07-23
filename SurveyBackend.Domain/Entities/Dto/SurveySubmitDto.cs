using System.Collections.Generic;

namespace SurveyBackend.Domain.Entities.Dto
{
    public class SurveySubmitDto
    {
        public List<SurveyAnswer> Answers { get; set; } = [];
    }

    public class SurveyAnswer
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; } = string.Empty;
    }
}
