using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IExamQuestionRepository : IRepositoryBase<ExamQuestion>
    {
        Task<bool> ExistsAsync(Guid examId, Guid questionId);
        Task<IEnumerable<ExamQuestion>> GetByExamIdAsync(Guid examId);
        Task<bool> AddQuestionsToExamAsync(Guid examId, IEnumerable<Guid> questionIds);
        Task<int> CountQuestionsInExamAsync(Guid examId); 
    }
}