using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        #region [- Ctor() -]
        public QuestionRepository(ProjectDbContext context) : base(context) { }
        #endregion

        #region [- GetAllQuestionsAsync() -]
        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _dbContext.Questions.ToListAsync();
        }
        #endregion

        #region [- GetQuestionWithOptionsAsync() -]
        public async Task<Question?> GetQuestionWithOptionsAsync(string id)
        {
            var question = await _dbContext.Questions
            .Include(q => q.QuestionOptions)
            .FirstOrDefaultAsync(q => q.Id == id);
            return question;
        }
        #endregion

        #region [- GetQuestionsByDifficultyAsync() -]
        public async Task<IEnumerable<Question>> GetQuestionsByDifficultyAsync(string difficultyLevel)
        {
            return await _dbContext.Questions
                .Where(q => q.DifficultyLevel == difficultyLevel)
                .ToListAsync();
        }
        #endregion

        #region [- HasCorrectOptionAsync() -]
        public async Task<bool> HasCorrectOptionAsync(string questionId)
        {
            return await _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .Join(_dbContext.Options,
                    qo => qo.OptionId,
                    o => o.Id,
                    (qo, o) => o)
                .AnyAsync(o => o.IsCorrect);
        }
        #endregion

        #region [- CountByDifficultyAsync() -]
        public async Task<int> CountByDifficultyAsync(string difficultyLevel)
        {
            return await _dbContext.Questions
                .CountAsync(q => q.DifficultyLevel == difficultyLevel);
        }
        #endregion


    }
}