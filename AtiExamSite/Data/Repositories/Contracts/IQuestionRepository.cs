using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsWithAnswersAsync();
        Task<Question> GetQuestionWithAnswersAsync(Guid id);
        Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(Guid examId);
    }
}