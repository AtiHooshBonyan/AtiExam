using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IExamSessionService
    {
        Task<ExamSession> GetSessionAsync(string examId);
        Task<bool> CreateAsync(ExamSession session);
        Task<bool> UpdateAsync(ExamSession session);
    }
}
