using Microsoft.AspNetCore.Mvc;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities.Dto;

namespace SurveyBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _surveyService;

    public SurveyController(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignSurvey([FromBody] SurveyAssignDto dto)
    {
        try
        {
            var survey = await _surveyService.AssignSurveyAsync(dto);

            // Build the link
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var surveyLink = $"{baseUrl}/survey/fill?token={survey.token}";

            return Ok(new
            {
                message = "Survey assigned successfully.",
                survey_link = surveyLink,
                valid_until = survey.valid_until
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    [HttpGet("fill/{token}")]
    public async Task<IActionResult> GetSurveyFillByToken(string token)
    {
        try
        {
            var survey = await _surveyService.GetSurveyFillByTokenAsync(token);

            if (survey == null)
                return BadRequest(new { message = "Survey link is invalid, expired, or already completed." });

            return Ok(survey);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("fill/{token}/submit")]
    public async Task<IActionResult> SubmitSurveyResponses(string token, [FromBody] SurveySubmitDto submitDto)
    {
        var result = await _surveyService.SubmitSurveyResponsesAsync(token, submitDto);

        if (!result)
            return BadRequest(new { message = "Failed to submit survey. It may be expired or already completed." });

        return Ok(new { message = "Survey submitted successfully!" });
    }

    [HttpPost("assign-multiple")]
    public async Task<IActionResult> AssignMultipleSurveys([FromBody] MultiSurveyAssignDto dto)
    {
        try
        {
            var result = await _surveyService.AssignMultipleSurveysAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error assigning multiple surveys: {ex.Message}");
        }
    }
    
    [HttpGet("responses/grouped-by-department")]
    public async Task<IActionResult> GetSurveyResponsesGroupedByDepartment()
    {
        try
        {
            var results = await _surveyService.GetSurveyResponsesGroupedByDepartmentAsync();
            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving grouped responses: {ex.Message}");
        }
    }
    
    [HttpGet("pending")]
    public async Task<IActionResult> GetAllPendingSurveys()
    {
        try
        {
            var pending = await _surveyService.GetAllPendingSurveysAsync();
            return Ok(pending);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving pending surveys: {ex.Message}");
        }
    }
}