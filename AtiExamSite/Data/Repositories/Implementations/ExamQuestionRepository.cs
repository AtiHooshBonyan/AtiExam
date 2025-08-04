using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class ExamQuestionRepository : RepositoryBase<ExamQuestion>, IExamQuestionRepository
    {
        #region [- Ctor() -]
        public ExamQuestionRepository(ProjectDbContext context) : base(context) { }
        #endregion

        #region [- ExistsAsync() -]
        public async Task<bool> ExistsAsync(Guid examId, Guid questionId)
        {
            return await _dbContext.ExamQuestions
                .AnyAsync(eq => eq.ExamId == examId && eq.QuestionId == questionId);
        }
        #endregion

        #region [- GetByExamIdAsync() -]
        public async Task<IEnumerable<ExamQuestion>> GetByExamIdAsync(Guid examId)
        {
            return await _dbContext.ExamQuestions
            .Where(eq => eq.ExamId == examId)
            .Include(eq => eq.Question)
            .ThenInclude(q => q.QuestionOptions)
            .ThenInclude(qo => qo.Option) 
            .ToListAsync();

        }

        #endregion

        #region [- AddQuestionsToExamAsync() -]
        public async Task<bool> AddQuestionsToExamAsync(Guid examId, IEnumerable<Guid> questionIds)
        {
            var existingQuestions = await _dbContext.ExamQuestions
                .Where(eq => eq.ExamId == examId)
                .Select(eq => eq.QuestionId)
                .ToListAsync();

            var newQuestions = questionIds
                .Except(existingQuestions)
                .Select(questionId => new ExamQuestion { ExamId = examId, QuestionId = questionId });

            await _dbContext.ExamQuestions.AddRangeAsync(newQuestions);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- CountQuestionsInExamAsync() -]
        public async Task<int> CountQuestionsInExamAsync(Guid examId)
        {
            return await _dbContext.ExamQuestions
                .CountAsync(eq => eq.ExamId == examId);
        } 
        #endregion
    }
}