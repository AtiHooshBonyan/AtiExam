using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using AtiExamSite.Services.Contracts;

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
        public async Task<bool> AddOptionsToQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds)
        {
            if (questionId == Guid.Empty) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));
            if (optionIds == null || !optionIds.Any())
                throw new ArgumentException("Option IDs cannot be null or empty", nameof(optionIds));

            return await _questionOptionRepository.AddOptionsToQuestionAsync(questionId, optionIds);
        }
        #endregion

        #region [- GetByQuestionIdAsync() -]
        public async Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(Guid questionId)
        {
            if (questionId == Guid.Empty) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));
            return await _questionOptionRepository.GetByQuestionIdAsync(questionId);
        }
        #endregion

        #region [- RemoveOptionsFromQuestionAsync() -]
        public async Task<bool> RemoveOptionsFromQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds)
        {
            if (questionId == Guid.Empty) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));
            if (optionIds == null || !optionIds.Any())
                throw new ArgumentException("Option IDs cannot be null or empty", nameof(optionIds));

            return await _questionOptionRepository.RemoveOptionsFromQuestionAsync(questionId, optionIds);
        } 
        #endregion
    }
}