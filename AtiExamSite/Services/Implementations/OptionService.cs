using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;

namespace AtiExamSite.Services.Implementations
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepository;

        #region [- Ctor() -]
        public OptionService(IOptionRepository optionRepository)
        {
            _optionRepository = optionRepository ?? throw new ArgumentNullException(nameof(optionRepository));
        }
        #endregion

        #region [- GetAllOptionsAsync() -]
        public async Task<IEnumerable<Option>> GetAllOptionsAsync()
        {
            return await _optionRepository.GetAllAsync();
        }
        #endregion

        #region [- GetByIdAsync() -]
        public async Task<Option?> GetByIdAsync(string id)
        {
            return await _optionRepository.GetByIdAsync(id);
        }
        #endregion

        #region [- CreateOptionAsync() -]
        public async Task<bool> CreateOptionAsync(Option option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));

            // If Id is not auto-generated, set it manually
            if (string.IsNullOrEmpty(option.Id))
            {
                option.Id = Guid.NewGuid().ToString(); // Generate a unique ID
            }

            await _optionRepository.AddAsync(option);
            return await _optionRepository.SaveChangesAsync();
        }
        #endregion

        #region [- GetOptionsByQuestionAsync() -]
        public async Task<IEnumerable<Option>> GetOptionsByQuestionAsync(string questionId)
        {
            if (questionId == string.Empty) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));
            return await _optionRepository.GetOptionsByQuestionAsync(questionId);
        }
        #endregion

        #region [- GetCorrectOptionForQuestionAsync() -]
        public async Task<Option?> GetCorrectOptionForQuestionAsync(string questionId)
        {
            if (questionId == string.Empty) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));
            return await _optionRepository.GetCorrectOptionForQuestionAsync(questionId);
        }
        #endregion

        #region [- IsCorrectOptionAsync() -]
        public async Task<bool> IsCorrectOptionAsync(string optionId)
        {
            if (optionId == string.Empty) throw new ArgumentException("Option ID cannot be empty", nameof(optionId));
            return await _optionRepository.IsCorrectOptionAsync(optionId);
        }
        #endregion

        #region [- UpdateOptionAsync() -]
        public async Task<bool> UpdateOptionAsync(Option option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));

            await _optionRepository.UpdateAsync(option);
            return await _optionRepository.SaveChangesAsync();
        }
        #endregion

        //the id must get cleared first then delete the whole record (application craash warning)
        #region [- DeleteOptionAsync() -]
        public async Task<bool> DeleteOptionAsync(string optionId)
        {
            if (optionId == string.Empty) throw new ArgumentException("Option ID cannot be empty", nameof(optionId));

            var option = await _optionRepository.GetByIdAsync(optionId);
            if (option == null) return false;

            _optionRepository.DeleteAsync(option);
            return await _optionRepository.SaveChangesAsync();
        }
        #endregion

    }
}