using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionWithOptionsAsync(string id);
        Task<IEnumerable<Question>> GetQuestionsByDifficultyAsync(string difficultyLevel);
        Task<bool> HasCorrectOptionAsync(string questionId);
        Task<int> CountByDifficultyAsync(string difficultyLevel); 

    }
}