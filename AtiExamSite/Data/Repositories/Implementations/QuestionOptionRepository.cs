using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class QuestionOptionRepository : RepositoryBase<QuestionOption>, IQuestionOptionRepository
    {
        #region [- Ctor() -]
        public QuestionOptionRepository(ProjectDbContext context) : base(context) { }
        #endregion

        #region [- GetByQuestionIdAsync() -]
        public async Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(Guid questionId)
        {
            return await _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .ToListAsync();
        }
        #endregion

        #region [- AddOptionsToQuestionAsync() -]
        public async Task<bool> AddOptionsToQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds)
        {
            if (optionIds == null || !optionIds.Any())
                return false;

            var existingOptions = await _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .Select(qo => qo.OptionId)
                .ToListAsync();

            var newOptions = optionIds
                .Except(existingOptions)
                .Select(optionId => new QuestionOption { QuestionId = questionId, OptionId = optionId });

            await _dbContext.QuestionOptions.AddRangeAsync(newOptions);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- RemoveOptionsFromQuestionAsync() -]
        public async Task<bool> RemoveOptionsFromQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds)
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