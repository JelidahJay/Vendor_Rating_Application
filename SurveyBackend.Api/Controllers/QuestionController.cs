using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBanckend.Infrastructure;

namespace SurveyBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _service;
    private readonly DataContext _context;

    public QuestionsController(IQuestionService service, DataContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _service.GetAllQuestionsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to fetch questions", detail = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _service.GetQuestionByIdAsync(id);
            return result is not null ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to fetch question", detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] QuestionCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Check if the same question_text exists
            var exists = await _context.questions.AnyAsync(q => q.question_text.ToLower() == dto.question_text.ToLower());
            if (exists)
                return Conflict(new { message = "Question already exists." });

            var result = await _service.CreateQuestionAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.question_id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to create question", detail = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] QuestionCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Check if the updated text conflicts with another question
            var duplicate = await _context.questions.AnyAsync(q => q.question_id != id && q.question_text.ToLower() == dto.question_text.ToLower());
            if (duplicate)
                return Conflict(new { message = "Another question with this text already exists." });

            var updated = await _service.UpdateQuestionAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to update question", detail = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteQuestionAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to delete question", detail = ex.Message });
        }
    }
}
