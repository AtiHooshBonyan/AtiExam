using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IQuestionService
    {
        Task<Question?> GetWithOptionsAsync(Guid id);
        Task<List<Question>> GetAllQuestionsAsync();
        Task<IEnumerable<Question>> GetByDifficultyAsync(string difficultyLevel);
        Task<bool> CreateQuestionAsync(Question question);
        Task<bool> CreateQuestionsAsync(IEnumerable<Question> questions);
        Task<bool> HasCorrectOptionAsync(Guid questionId);
        Task<int> CountByDifficultyAsync(string difficultyLevel);
        Task<bool> SetCorrectOptionAsync(Guid questionId, Guid correctOptionId);
        Task<Question?> GetQuestionByIdAsync(Guid id);
        Task<bool> UpdateQuestionAsync(Question question);
        Task<bool> DeleteQuestionAsync(Guid questionId);

    }
}
