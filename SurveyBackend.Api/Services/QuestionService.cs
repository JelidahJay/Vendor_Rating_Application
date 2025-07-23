using Microsoft.EntityFrameworkCore;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBackend.Domain.Entities.Dto;
using SurveyBanckend.Infrastructure;

namespace SurveyBackend.Api.Services;

public class QuestionService : IQuestionService
{
    private readonly DataContext _context;

    public QuestionService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync()
    {

        var questions = await _context.questions
            .Include(q => q.question_type)
            .Include(q => q.options!)
            .ThenInclude(qo => qo.option)
            .OrderBy(q => q.question_order)
            .ToListAsync();

        foreach (var q in questions)
        {
            Console.WriteLine($"Question: {q.question_text}");
            foreach (var o in q.options!)
            {
                Console.WriteLine($"  Option: {o.option?.option_text}");
            }
        }



        return questions.Select(q => new QuestionDto
        {
            question_id = q.question_id,
            question_text = q.question_text,
            is_required = q.is_required,
            question_order = q.question_order,
            question_type = q.question_type?.type_name ?? "",
            options = q.options?.Select(o => o.option!.option_text).ToList() ?? []
        });

    }

    public async Task<QuestionDto?> GetQuestionByIdAsync(int id)
    {
        var q = await _context.questions
            .Include(q => q.question_type)
            .Include(q => q.options!)
                .ThenInclude(o => o.option)
            .FirstOrDefaultAsync(q => q.question_id == id);

        if (q is null) return null;

        return new QuestionDto
        {
            question_id = q.question_id,
            question_text = q.question_text,
            is_required = q.is_required,
            question_order = q.question_order,
            question_type = q.question_type?.type_name ?? "",
            options = q.options?.Select(o => o.option!.option_text).ToList() ?? []
        };
    }

    public async Task<QuestionDto> CreateQuestionAsync(QuestionCreateDto dto)
    {
        var question = new Questions
        {
            question_text = dto.question_text,
            is_required = dto.is_required,
            question_order = dto.question_order,
            question_type_id = dto.question_type_id,
            options = []
        };

        _context.questions.Add(question);
        await _context.SaveChangesAsync(); // save to get question_id

        // Attach options if provided
        if (dto.options is not null && dto.options.Any())
        {
            foreach (var text in dto.options)
            {
                var option = await _context.options
                    .FirstOrDefaultAsync(o => o.option_text == text)
                    ?? new Option { option_text = text };

                _context.question_options.Add(new QuestionOption
                {
                    question_id = question.question_id,
                    option_id = option.option_id > 0 ? option.option_id : 0,
                    option = option
                });
            }

            await _context.SaveChangesAsync();
        }

        return await GetQuestionByIdAsync(question.question_id) ?? throw new Exception("Creation failed");
    }

    public async Task<bool> UpdateQuestionAsync(int id, QuestionCreateDto dto)
    {
        var question = await _context.questions
            .Include(q => q.options)
            .FirstOrDefaultAsync(q => q.question_id == id);

        if (question is null) return false;

        question.question_text = dto.question_text;
        question.is_required = dto.is_required;
        question.question_order = dto.question_order;
        question.question_type_id = dto.question_type_id;

        // Remove old options
        _context.question_options.RemoveRange(question.options ?? []);
        await _context.SaveChangesAsync();

        // Add new options
        if (dto.options is not null && dto.options.Any())
        {
            foreach (var text in dto.options)
            {
                var option = await _context.options
                    .FirstOrDefaultAsync(o => o.option_text == text)
                    ?? new Option { option_text = text };

                _context.question_options.Add(new QuestionOption
                {
                    question_id = question.question_id,
                    option_id = option.option_id > 0 ? option.option_id : 0,
                    option = option
                });
            }

            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> DeleteQuestionAsync(int id)
    {
        var question = await _context.questions
            .Include(q => q.options)
            .FirstOrDefaultAsync(q => q.question_id == id);

        if (question is null) return false;

        _context.question_options.RemoveRange(question.options ?? []);
        _context.questions.Remove(question);
        await _context.SaveChangesAsync();

        return true;
    }
}
