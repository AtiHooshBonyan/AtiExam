using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class ExamRepository : RepositoryBase<Exam>, IExamRepository
    {
        #region [- Ctor() -]
        public ExamRepository(ProjectDbContext context) : base(context) { }
        #endregion

        #region [- GetExamsByUserAsync() -]
        public async Task<IReadOnlyCollection<Exam>> GetExamsByUserAsync(Guid userId)
        {
            return await _dbContext.Exams
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }
        #endregion

        #region [- ExistsAsync() -]
        public async Task<bool> ExistsAsync(string title, Guid? excludeExamId = null)
        {
            var query = _dbContext.Exams.Where(e => e.Title == title);

            if (excludeExamId.HasValue)
            {
                query = query.Where(e => e.Id != excludeExamId.Value);
            }

            return await query.AnyAsync();
        }
        #endregion

        #region [- GetExamWithQuestionsAsync() -]
        public async Task<Exam?> GetExamWithQuestionsAsync(Guid examId)
        {
            return await _dbContext.Exams
                .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                .FirstOrDefaultAsync(e => e.Id == examId);
        }
        #endregion

        #region [- GetRandomQuestionsAsync() -]
        public async Task<IReadOnlyCollection<Question>> GetRandomQuestionsAsync(int count)
        {
            if (count <= 0)
                return new List<Question>();

            return await _dbContext.Questions
                .OrderBy(q => Guid.NewGuid())
                .Take(count)
                .ToListAsync();
        }
        #endregion

        #region [- IsActiveAsync() -]
        public async Task<bool> IsActiveAsync(Guid examId)
        {
            var exam = await GetByIdAsync(examId);
            return exam?.IsActive ?? false;
        } 
        #endregion
    }
}