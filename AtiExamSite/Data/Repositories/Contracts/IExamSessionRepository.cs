using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IExamSessionRepository : IRepositoryBase<ExamSession>
    {
        Task<IEnumerable<ExamSession>> GetSessionsByUserAsync(string userId);
        Task<ExamSession> GetSessionWithDetailsAsync(Guid sessionId);
        Task<IEnumerable<ExamSession>> GetSessionsByExamAsync(Guid examId);
        Task<ExamSession> StartNewSessionAsync(Guid examId, string userId);
        Task<bool> CompleteSessionAsync(Guid sessionId);
        Task<bool> CheckAndEndExpiredSessionsAsync();
        Task<bool> IsSessionActiveAsync(Guid sessionId);
        Task<bool> HasUserTakenExamAsync(string userId, Guid examId);
    }
}