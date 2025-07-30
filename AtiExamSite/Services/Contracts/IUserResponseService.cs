using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Services.Contracts
{
    public interface IUserResponseService
    {
        Task<bool> SubmitResponsesAsync(IEnumerable<UserResponse> responses);
        Task<IReadOnlyCollection<UserResponse>> GetByExamAsync(Guid examId);
        //Task<IReadOnlyCollection<UserResponse>> GetByUserAsync(Guid userId);
        Task<double> CalculateExamScoreAsync(Guid examId);
        //Task<bool> HasUserTakenExamAsync(Guid userId, Guid examId);
    }
}