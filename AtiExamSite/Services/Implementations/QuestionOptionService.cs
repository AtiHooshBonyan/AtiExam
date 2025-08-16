using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Services.Implementations
{
    public class QuestionOptionService : IQuestionOptionService
    {
        private readonly IQuestionOptionRepository _questionOptionRepository;

        #region [- Ctor() -]
        public QuestionOptionService(IQuestionOptionRepository questionOptionRepository)
        {
            _questionOptionRepository = questionOptionRepository ?? throw new ArgumentNullException(nameof(questionOptionRepository));
        }
        #endregion

        #region [- AddOptionsToQuestionAsync() -]
        public async Task<bool> AddOptionsToQuestionAsync(string questionId, IEnumerable<string> optionIds)
        {
            foreach (var optionId in optionIds)
            {
                var qo = new QuestionOption
                {
                    Id = Guid.NewGuid().ToString(),
                    QuestionId = questionId,
                    OptionId = optionId
                };

                await _questionOptionRepository.AddAsync(qo);
            }

            await _questionOptionRepository.SaveChangesAsync();
            return true;
        }

        #endregion

        #region [- GetByQuestionIdAsync() -]
        public async Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(string questionId)
        {
            if (questionId == Guid.Empty.ToString()) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));
            return await _questionOptionRepository.GetByQuestionIdAsync(questionId);
        }
        #endregion

        #region [- RemoveOptionsFromQuestionAsync() -]
        public async Task<bool> RemoveOptionsFromQuestionAsync(string questionId, IEnumerable<string> optionIds)
        {
            if (questionId == Guid.Empty.ToString()) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));
            if (optionIds == null || !optionIds.Any())
                throw new ArgumentException("Option IDs cannot be null or empty", nameof(optionIds));

            return await _questionOptionRepository.RemoveOptionsFromQuestionAsync(questionId, optionIds);
        } 
        #endregion
    }
}