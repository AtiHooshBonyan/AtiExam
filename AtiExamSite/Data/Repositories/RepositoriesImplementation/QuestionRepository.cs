using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.RepositoriesImplementation
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        #region [- Ctor() -]
        public QuestionRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        } 
        #endregion

        #region [- GetQuestionsWithAnswersAsync() -]
        public async Task<IEnumerable<Question>> GetQuestionsWithAnswersAsync()
        {
            return await _dbContext.Questions
                .Include(q => q.AnswerOptions)
                .AsNoTracking()
                .ToListAsync();
        } 
        #endregion

        #region [- GetQuestionWithAnswersAsync() -]
        public async Task<Question> GetQuestionWithAnswersAsync(Guid id)
        {
            return await _dbContext.Questions
                .Include(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.QuestionId == id);
        } 
        #endregion

        #region [- GetQuestionsByExamIdAsync() -]
        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(Guid examId)
        {
            return await _dbContext.Questions
                .Where(q => q.ExamId == examId)
                .Include(q => q.AnswerOptions)
                .AsNoTracking()
                .ToListAsync();
        } 
        #endregion
    }
}