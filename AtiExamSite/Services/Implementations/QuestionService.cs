using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Data.Repositories.Implementations;
using AtiExamSite.Models.DomainModels;
using AtiExamSite.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AtiExamSite.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IUserResponseRepository _userResponseRepository;

        #region [- Ctor() -]
        public QuestionService(IQuestionRepository questionRepository, IUserResponseRepository userResponseRepository, IQuestionOptionRepository questionOptionRepository, IOptionRepository optionRepository)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _userResponseRepository=userResponseRepository;
            _questionOptionRepository=questionOptionRepository;
            _optionRepository=optionRepository;
        }
        #endregion

        #region [- GetWithOptionsAsync() -]
        public async Task<Question?> GetWithOptionsAsync(Guid id)
        {
            return await _questionRepository.GetQuestionWithOptionsAsync(id);
        }
        #endregion

        #region [- GetAllQuestionsAsync() -]
        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllQuestionsAsync();
            
        }

        #endregion

        #region [- GetByDifficultyAsync() -]
        public async Task<IEnumerable<Question>> GetByDifficultyAsync(string difficultyLevel)
        {
            return await _questionRepository.GetQuestionsByDifficultyAsync(difficultyLevel);
        }
        #endregion

        #region [- GetQuestionByIdAsync() -]
        public async Task<Question?> GetQuestionByIdAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid question ID", nameof(id));

            return await _questionRepository.GetByIdAsync(id);
        }
        #endregion

        #region [- UpdateQuestionAsync() -]
        public async Task<bool> UpdateQuestionAsync(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            var existing = await _questionRepository.GetByIdAsync(question.Id);
            if (existing == null) return false;

            
            existing.Title = question.Title;
            existing.DifficultyLevel = question.DifficultyLevel;
            existing.DifficultyLevel = question.DifficultyLevel;

            await _questionRepository.UpdateAsync(existing);
            return await _questionRepository.SaveChangesAsync();
        }
        #endregion

        #region [- DeleteQuestionAsync() -]
        public async Task<bool> DeleteQuestionAsync(Guid questionId)
        {
            if (questionId == Guid.Empty) throw new ArgumentException("Invalid question ID", nameof(questionId));

            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null) return false;

            // Delete related UserResponses first to avoid FK conflict
            await _userResponseRepository.DeleteByQuestionIdAsync(questionId);

            // Now delete the question itself
            await _questionRepository.DeleteAsync(question);
            return await _questionRepository.SaveChangesAsync();
        }
        #endregion

        #region [- CreateQuestionAsync() -]
        public async Task<bool> CreateQuestionAsync(Question question)
        {

            await _questionRepository.AddAsync(question);

            var result = await _questionRepository.SaveChangesAsync();

            return result;
        }
        #endregion

        #region [- CreateBulkQuestionsAsync() -]
        public async Task<bool> CreateQuestionsAsync(IEnumerable<Question> questions)
        {
            await _questionRepository.AddRangeAsync(questions);

            var result = await _questionRepository.SaveChangesAsync();

            return result;
        }
        #endregion

        #region [- HasCorrectOptionAsync() -]
        public async Task<bool> HasCorrectOptionAsync(Guid questionId)
        {
            return await _questionRepository.HasCorrectOptionAsync(questionId);
        }
        #endregion

        #region [- CountByDifficultyAsync() -]
        public async Task<int> CountByDifficultyAsync(string difficultyLevel)
        {
            return await _questionRepository.CountByDifficultyAsync(difficultyLevel);
        }
        #endregion

        #region [- SetCorrectOptionAsync() -]
        public async Task<bool> SetCorrectOptionAsync(Guid questionId, Guid correctOptionId)
        {
            var questionOptions = await _questionOptionRepository.GetByQuestionIdAsync(questionId);
            var targetQo = questionOptions.FirstOrDefault(qo => qo.OptionId == correctOptionId);
            if (targetQo == null)
                return false;

            var optionIds = questionOptions.Select(qo => qo.OptionId).ToList();
            var options = await _optionRepository.GetByIdsAsync(optionIds);
            if (options == null || !options.Any())
                return false;

            foreach (var opt in options)
            {
                opt.IsCorrect = opt.Id == correctOptionId;
                _optionRepository.UpdateAsync(opt);
            }

            return await _optionRepository.SaveChangesAsync();
        }
        #endregion


    }
}