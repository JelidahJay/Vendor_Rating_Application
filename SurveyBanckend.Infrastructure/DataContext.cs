using Microsoft.EntityFrameworkCore;
using SurveyBackend.Domain.Entities;

namespace SurveyBanckend.Infrastructure;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Questions> questions => Set<Questions>();
    public DbSet<QuestionType> question_types => Set<QuestionType>();
    public DbSet<QuestionOption> question_options => Set<QuestionOption>();
    public DbSet<Option> options => Set<Option>(); // NEW: Option master table

    public DbSet<Department> departments => Set<Department>();
    public DbSet<User> users => Set<User>();
    public DbSet<Vendor> vendors => Set<Vendor>();
    public DbSet<Survey> surveys => Set<Survey>();
    public DbSet<SurveyResponse> survey_responses => Set<SurveyResponse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.department)
            .WithMany(d => d.users)
            .HasForeignKey(u => u.department_id);

        modelBuilder.Entity<Survey>()
            .HasOne(s => s.vendor)
            .WithMany(v => v.surveys)
            .HasForeignKey(s => s.vendor_id);

        modelBuilder.Entity<Survey>()
            .HasOne(s => s.user)
            .WithMany(u => u.surveys_rated)
            .HasForeignKey(s => s.user_id);

        modelBuilder.Entity<Survey>()
            .HasOne(s => s.invited_by)
            .WithMany(u => u.surveys_invited)
            .HasForeignKey(s => s.invited_by_user_id);

        modelBuilder.Entity<SurveyResponse>()
            .HasOne(r => r.survey)
            .WithMany(s => s.responses)
            .HasForeignKey(r => r.survey_id);

        modelBuilder.Entity<SurveyResponse>()
            .HasOne(r => r.question)
            .WithMany()
            .HasForeignKey(r => r.question_id);
        
        modelBuilder.Entity<Questions>()
            .HasOne(q => q.question_type)
            .WithMany(t => t.questions)
            .HasForeignKey(q => q.question_type_id);

        modelBuilder.Entity<QuestionOption>()
            .HasOne(qo => qo.question)
            .WithMany(q => q.options)
            .HasForeignKey(qo => qo.question_id);

        modelBuilder.Entity<QuestionOption>()
            .HasOne(qo => qo.option)
            .WithMany()
            .HasForeignKey(qo => qo.option_id);

    }

}