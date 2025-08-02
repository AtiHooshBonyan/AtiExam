using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Services.Contracts
{
    public interface IExamSessionService
    {
        Task<ExamSession> GetSessionAsync( Guid examId);
        Task<bool> CreateAsync(ExamSession session);
        Task<bool> UpdateAsync(ExamSession session);
    }
}
