using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionWithOptionsAsync(Guid id);
        Task<IEnumerable<Question>> GetQuestionsByDifficultyAsync(string difficultyLevel);
        Task<bool> HasCorrectOptionAsync(Guid questionId);
        Task<int> CountByDifficultyAsync(string difficultyLevel); 

    }
}