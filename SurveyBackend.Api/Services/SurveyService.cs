using Microsoft.EntityFrameworkCore;
using SurveyBanckend.Infrastructure;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBackend.Domain.Entities.Dto;

namespace SurveyBackend.Api.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly DataContext _context;

        public SurveyService(DataContext context)
        {
            _context = context;
        }

        public async Task<Survey> AssignSurveyAsync(SurveyAssignDto dto)
        {
            var survey = new Survey
            {
                vendor_id = dto.vendor_id,
                user_id = dto.user_id,
                invited_by_user_id = dto.invited_by_user_id,
                token = Guid.NewGuid().ToString(), // 🔥 Generate random secure token
                valid_until = DateTime.UtcNow.AddDays(dto.valid_days),
                status = "pending"
            };

            _context.surveys.Add(survey);
            await _context.SaveChangesAsync();
            return survey;
        }

        public async Task<Survey?> GetSurveyByTokenAsync(string token)
        {
            return await _context.surveys
                .Include(s => s.vendor)
                .Include(s => s.user)
                .FirstOrDefaultAsync(s => s.token == token);
        }
        
        public async Task<SurveyFillDto?> GetSurveyFillByTokenAsync(string token)
        {
            var survey = await _context.surveys
                .Include(s => s.vendor)
                .Include(s => s.user)
                .FirstOrDefaultAsync(s => s.token == token);

            if (survey == null || survey.status == "completed" || survey.valid_until < DateTime.UtcNow)
                return null;

            var questions = await _context.questions
                .Include(q => q.question_type)
                .Include(q => q.options!)
                .ThenInclude(qo => qo.option)
                .OrderBy(q => q.question_order)
                .ToListAsync();

            return new SurveyFillDto
            {
                rater_name = survey.user?.full_name ?? "Unknown",
                rater_email = survey.user?.email ?? "Unknown",
                vendor_name = survey.vendor?.name ?? "Unknown",
                survey_questions = questions.Select(q => new QuestionDto
                {
                    question_id = q.question_id,
                    question_text = q.question_text,
                    question_type = q.question_type?.type_name ?? "",
                    options = q.options?.Select(o => o.option!.option_text).ToList() ?? []
                }).ToList()
            };
        }
        
        public async Task<bool> SubmitSurveyResponsesAsync(string token, SurveySubmitDto submitDto)
        {
            Console.WriteLine("📨 Received submit request for token: " + token);

            var survey = await _context.surveys
                .Include(s => s.responses)
                .FirstOrDefaultAsync(s => s.token == token && s.status == "pending");

            if (survey == null)
            {
                Console.WriteLine("🚨 Survey not found or already completed for token: " + token);
                return false;
            }

            Console.WriteLine($"✅ Survey found. Survey ID: {survey.survey_id}");

            // Save each response
            foreach (var answer in submitDto.Answers)
            {
                Console.WriteLine($"📝 Saving answer for QuestionId: {answer.QuestionId} - Answer: {answer.Answer}");

                var response = new SurveyResponse
                {
                    survey_id = survey.survey_id,
                    question_id = answer.QuestionId,
                    answer = answer.Answer
                };

                _context.survey_responses.Add(response);
            }

            // Mark survey as completed
            survey.status = "completed";
            survey.submitted_at = DateTime.UtcNow;

            Console.WriteLine($"✅ Marked survey as completed at {survey.submitted_at}");

            await _context.SaveChangesAsync();

            Console.WriteLine("✅✅ All survey responses saved successfully!");

            return true;
        }

        public async Task<List<Survey>> AssignMultipleSurveysAsync(MultiSurveyAssignDto dto)
        {
            var surveys = new List<Survey>();

            foreach (var userId in dto.user_ids)
            {
                var survey = new Survey
                {
                    vendor_id = dto.vendor_id,
                    user_id = userId,
                    invited_by_user_id = dto.invited_by_user_id,
                    token = Guid.NewGuid().ToString(),
                    valid_until = DateTime.UtcNow.AddDays(dto.valid_days),
                    status = "pending"
                };

                surveys.Add(survey);
            }

            _context.surveys.AddRange(surveys);
            await _context.SaveChangesAsync();

            return surveys;
        }
        
        public async Task<List<DepartmentSurveySummaryDto>> GetSurveyResponsesGroupedByDepartmentAsync()
        {
            var departments = await _context.departments
                .Include(d => d.users!)
                .ThenInclude(u => u.surveys_rated!)
                .ThenInclude(s => s.vendor)
                .AsSplitQuery() // optional, but improves stability
                .ToListAsync();

            var result = departments.Select(d => new DepartmentSurveySummaryDto
            {
                department_name = d.name,
                surveys = d.users
                    .SelectMany(u => u.surveys_rated ?? new List<Survey>())
                    .Where(s => s.status == "completed")
                    .Select(s => new SurveySummaryDto
                    {
                        rater_name = s.user?.full_name ?? "Unknown", // this will still work
                        rater_email = s.user?.email ?? "Unknown",
                        vendor_name = s.vendor?.name ?? "Unknown",
                        submitted_at = s.submitted_at
                    }).ToList()
            }).ToList();
            
            return result;
        }

        public async Task<List<PendingSurveyDto>> GetAllPendingSurveysAsync()
        {
            return await _context.surveys
                .Where(s => s.status == "pending")
                .Include(s => s.user)
                .Include(s => s.vendor)
                .Select(s => new PendingSurveyDto
                {
                    survey_id = s.survey_id,
                    token = s.token,
                    valid_until = s.valid_until,
                    rater_name = s.user.full_name,
                    rater_email = s.user.email,
                    vendor_name = s.vendor.name
                })
                .ToListAsync();
        }
    }
}