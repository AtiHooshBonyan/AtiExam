using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IQuestionService
    {
        Task<Question?> GetWithOptionsAsync(string id);
        Task<List<Question>> GetAllQuestionsAsync();
        Task<IEnumerable<Question>> GetByDifficultyAsync(string difficultyLevel);
        Task<bool> CreateQuestionAsync(Question question);
        Task<bool> CreateQuestionsAsync(IEnumerable<Question> questions);
        Task<bool> HasCorrectOptionAsync(string questionId);
        Task<int> CountByDifficultyAsync(string difficultyLevel);
        Task<bool> SetCorrectOptionAsync(string questionId, string correctOptionId);
        Task<Question?> GetQuestionByIdAsync(string id);
        Task<bool> UpdateQuestionAsync(Question question);
        Task<bool> DeleteQuestionAsync(string questionId);

    }
}
