using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBackend.Domain.Entities.Dto;

namespace SurveyBackend.Api.Interfaces;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync();
    Task<QuestionDto?> GetQuestionByIdAsync(int id);
    Task<QuestionDto> CreateQuestionAsync(QuestionCreateDto dto);
    Task<bool> UpdateQuestionAsync(int id, QuestionCreateDto dto);
    Task<bool> DeleteQuestionAsync(int id);
}
