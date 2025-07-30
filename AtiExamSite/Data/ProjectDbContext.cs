using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<UserResponse> UserResponses { get; set; }
        public DbSet<ExamSession> ExamSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity config
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
                entity.Property(u => u.IsAdmin).HasDefaultValue(false);
            });

            // Exam entity config
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // One-to-many: Exam has many ExamQuestions
                entity.HasMany(e => e.ExamQuestions)
                      .WithOne(eq => eq.Exam)
                      .HasForeignKey(eq => eq.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Question entity config
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Title).IsRequired().HasMaxLength(1000);
                entity.Property(q => q.DifficultyLevel).IsRequired().HasMaxLength(50);

                // One-to-many: Question has many QuestionOptions
                entity.HasMany(q => q.QuestionOptions)
                      .WithOne(qo => qo.Question)
                      .HasForeignKey(qo => qo.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-many: Question has many UserResponses
                entity.HasMany(q => q.UserResponses)
                      .WithOne(ur => ur.Question)
                      .HasForeignKey(ur => ur.QuestionId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // Option entity config
            modelBuilder.Entity<Option>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Title).IsRequired().HasMaxLength(500);
                entity.Property(o => o.IsCorrect).IsRequired();

                // One-to-many: Option has many QuestionOptions
                entity.HasMany(o => o.QuestionOptions)
                      .WithOne(qo => qo.Option)
                      .HasForeignKey(qo => qo.OptionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ExamQuestion entity config (join table)
            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasKey(eq => eq.Id);

                entity.HasOne(eq => eq.Exam)
                      .WithMany(e => e.ExamQuestions)
                      .HasForeignKey(eq => eq.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(eq => eq.Question)
                      .WithMany(q => q.ExamQuestions)
                      .HasForeignKey(eq => eq.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(eq => new { eq.ExamId, eq.QuestionId }).IsUnique();
            });

            // QuestionOption entity config (join table)
            modelBuilder.Entity<QuestionOption>(entity =>
            {
                entity.HasKey(qo => qo.Id);

                entity.HasOne(qo => qo.Question)
                      .WithMany(q => q.QuestionOptions)
                      .HasForeignKey(qo => qo.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(qo => qo.Option)
                      .WithMany(o => o.QuestionOptions)
                      .HasForeignKey(qo => qo.OptionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(qo => new { qo.QuestionId, qo.OptionId }).IsUnique();
            });

            // UserResponse entity config
            modelBuilder.Entity<UserResponse>(entity =>
            {
                entity.HasKey(ur => ur.Id);

                //entity.HasOne(ur => ur.User)
                //      .WithMany() // no navigation from User to UserResponses by default
                //      .HasForeignKey(ur => ur.UserId)
                //      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ur => ur.Exam)
                      .WithMany() // no navigation from Exam to UserResponses by default
                      .HasForeignKey(ur => ur.ExamId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ur => ur.Question)
                      .WithMany(q => q.UserResponses)
                      .HasForeignKey(ur => ur.QuestionId)
                      .OnDelete(DeleteBehavior.Restrict);

                //entity.HasIndex(ur => new { ur.UserId, ur.ExamId, ur.QuestionId }).IsUnique();
            });
        }
    }
}
