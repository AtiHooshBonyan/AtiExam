using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IUserResponseService
    {
        Task<bool> SubmitResponsesAsync(IEnumerable<UserResponse> responses);
        Task<IReadOnlyCollection<UserResponse>> GetByExamAsync(string examId);
        //Task<IReadOnlyCollection<UserResponse>> GetByUserAsync(Guid userId);
        Task<(double Score, bool Passed)> CalculateExamScoreAsync(string examId);
        //Task<bool> HasUserTakenExamAsync(Guid userId, Guid examId);
    }
}