using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IExamQuestionRepository : IRepositoryBase<ExamQuestion>
    {
        Task<bool> ExistsAsync(string examId, string questionId);
        Task<IEnumerable<ExamQuestion>> GetByExamIdAsync(string examId);
        Task<bool> AddQuestionsToExamAsync(string examId, IEnumerable<string> questionIds);
        Task<int> CountQuestionsInExamAsync(string examId); 
    }
}