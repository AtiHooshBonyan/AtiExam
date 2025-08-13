using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IExamSessionRepository
    {
        Task<ExamSession> GetByUserAndExamAsync(string examId);
        Task AddAsync(ExamSession session);
        Task UpdateAsync(ExamSession session);
    }
}
