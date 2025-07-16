using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IUserResponseRepository : IRepositoryBase<UserResponse>
    {
        Task<IEnumerable<UserResponse>> GetResponsesBySessionAsync(Guid sessionId);
        Task<IEnumerable<UserResponse>> GetResponsesByQuestionAsync(Guid questionId);
        Task<int> CalculateSessionScoreAsync(Guid sessionId);
        Task<int> GetCorrectAnswerCountAsync(Guid sessionId); // New method
        Task<int> GetTotalQuestionCountAsync(Guid sessionId); // New method
    }
}