using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IExamSessionRepository
    {
        Task<ExamSession> GetByUserAndExamAsync(Guid userId, Guid examId);
        Task AddAsync(ExamSession session);
        Task UpdateAsync(ExamSession session);
    }
}
