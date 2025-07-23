using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.Dto;

namespace SurveyBackend.Api.Interfaces;

public interface ISurveyService
{
    Task<Survey> AssignSurveyAsync(SurveyAssignDto dto);
    Task<Survey?> GetSurveyByTokenAsync(string token);
    
    Task<SurveyFillDto?> GetSurveyFillByTokenAsync(string token);

    Task<bool> SubmitSurveyResponsesAsync(string token, SurveySubmitDto submitDto);
    
    Task<List<Survey>> AssignMultipleSurveysAsync(MultiSurveyAssignDto dto);
    
    Task<List<DepartmentSurveySummaryDto>> GetSurveyResponsesGroupedByDepartmentAsync();
    
    Task<List<PendingSurveyDto>> GetAllPendingSurveysAsync();
}