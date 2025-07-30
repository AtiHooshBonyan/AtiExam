using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class OptionRepository : RepositoryBase<Option>, IOptionRepository
    {
        #region [- Ctor() -] 
        public OptionRepository(ProjectDbContext context) : base(context) { }
        #endregion

        #region [- GetOptionsByQuestionAsync() -]
        public async Task<IEnumerable<Option>> GetOptionsByQuestionAsync(Guid questionId)
        {
            return await _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .Join(_dbContext.Options,
                    qo => qo.OptionId,
                    o => o.Id,
                    (qo, o) => o)
                .ToListAsync();
        }
        #endregion

        #region [- GetCorrectOptionForQuestionAsync() -]
        public async Task<Option?> GetCorrectOptionForQuestionAsync(Guid questionId)
        {
            return await _dbContext.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .Join(_dbContext.Options,
                    qo => qo.OptionId,
                    o => o.Id,
                    (qo, o) => o)
                .FirstOrDefaultAsync(o => o.IsCorrect);
        }
        #endregion

        #region [- IsCorrectOptionAsync() -]
        public async Task<bool> IsCorrectOptionAsync(Guid optionId)
        {
            var option = await GetByIdAsync(optionId);
            return option?.IsCorrect ?? false;
        }
        #endregion

        #region [- UpdateOptionAsync() -]
        public async Task UpdateOptionAsync(Option updatedOption)
        {
            var existingOption = await _dbContext.Options.FindAsync(updatedOption.Id);
            if (existingOption is not null)
            {
                existingOption.Title = updatedOption.Title;
                existingOption.IsCorrect = updatedOption.IsCorrect;
            }
        } 
        #endregion

    }
}