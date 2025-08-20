using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class QuestionOptionRepository : RepositoryBase<QuestionOption>, IQuestionOptionRepository
    {
        #region [- Ctor() -]
        public QuestionOptionRepository(ProjectDbContext context) : base(context) { }
        #endregion

        #region [- GetByQuestionIdAsync() -]
        public async Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(string questionId)
        {
            return await _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .ToListAsync();
        }
        #endregion

        #region [- RemoveOptionsFromQuestionAsync() -]
        public async Task<bool> RemoveOptionsFromQuestionAsync(string questionId, IEnumerable<string> optionIds)
        {
            var optionsToRemove = await _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId && optionIds.Contains(qo.OptionId))
                .ToListAsync();

            _dbContext.QuestionOptions.RemoveRange(optionsToRemove);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion
    }
}