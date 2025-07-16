using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.RepositoriesImplementation
{
    public class AnswerOptionRepository : BaseRepository<AnswerOption>, IAnswerOptionRepository
    {
        #region [- Ctor() -]
        public AnswerOptionRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region [- GetAnswerOptionsByQuestionAsync() -]
        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsByQuestionAsync(Guid questionId)
        {
            return await _dbContext.AnswerOptions
                .Where(a => a.QuestionId == questionId)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region [- GetCorrectAnswersForQuestionAsync() -]
        public async Task<IEnumerable<AnswerOption>> GetCorrectAnswersForQuestionAsync(Guid questionId)
        {
            return await _dbContext.AnswerOptions
                .Where(a => a.QuestionId == questionId && a.IsCorrect)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}