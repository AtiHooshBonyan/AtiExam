using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<ExamSession> ExamSessions { get; set; }
        public DbSet<UserResponse> UserResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Exam
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.ExamId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.PassingScore).IsRequired();
                entity.Property(e => e.TimeLimitMinutes).IsRequired(false);
                entity.Property(e => e.QuestionsToShow).IsRequired();
            });

            // Configure Question
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.QuestionId);
                entity.Property(q => q.Text).IsRequired().HasMaxLength(1000);
                entity.Property(q => q.DifficultyLevel).HasMaxLength(50);
                entity.Property(q => q.Category).HasMaxLength(100);

                entity.HasOne(q => q.Exam)
                    .WithMany(e => e.Questions)
                    .HasForeignKey(q => q.ExamId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(q => q.ExamId);
            });

            // Configure AnswerOption
            modelBuilder.Entity<AnswerOption>(entity =>
            {
                entity.HasKey(a => a.AnswerId);
                entity.Property(a => a.Text).IsRequired().HasMaxLength(500);
                entity.Property(a => a.IsCorrect).IsRequired();

                entity.HasOne(a => a.Question)
                    .WithMany(q => q.AnswerOptions)
                    .HasForeignKey(a => a.QuestionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ExamSession
            modelBuilder.Entity<ExamSession>(entity =>
            {
                entity.HasKey(s => s.SessionId);
                entity.Property(s => s.UserId).IsRequired().HasMaxLength(450);
                entity.Property(s => s.Status).IsRequired().HasMaxLength(50);
                entity.Property(s => s.StartTime).IsRequired();
                entity.Property(s => s.Score).IsRequired(false);

                entity.HasOne(s => s.Exam)
                    .WithMany()
                    .HasForeignKey(s => s.ExamId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => s.UserId);
                entity.HasIndex(s => s.ExamId);
                entity.HasIndex(s => s.Status);
                entity.HasIndex(s => s.EndTime);
            });

            // Configure UserResponse
            modelBuilder.Entity<UserResponse>(entity =>
            {
                entity.HasKey(r => r.ResponseId);
                entity.Property(r => r.ResponseTime).IsRequired();
                entity.Property(r => r.IsCorrect).IsRequired(false);

                entity.HasOne(r => r.ExamSession)
                    .WithMany(s => s.UserResponses)
                    .HasForeignKey(r => r.SessionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Question)
                    .WithMany()
                    .HasForeignKey(r => r.QuestionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.AnswerOption)
                    .WithMany()
                    .HasForeignKey(r => r.AnswerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(r => r.SessionId);
                entity.HasIndex(r => r.QuestionId);
                entity.HasIndex(r => new { r.SessionId, r.QuestionId }).IsUnique();
            });
        }
    }
}
