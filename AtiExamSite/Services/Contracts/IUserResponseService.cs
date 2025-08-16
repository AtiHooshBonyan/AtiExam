using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IUserResponseService
    {
        Task<bool> SubmitResponsesAsync(IEnumerable<UserResponse> responses);
        Task<IReadOnlyCollection<UserResponse>> GetByExamAsync(string examId);
        Task<(double Score, bool Passed)> CalculateExamScoreAsync(string examId);
    }
}