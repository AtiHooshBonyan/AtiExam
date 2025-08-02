using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IUserResponseRepository : IRepositoryBase<UserResponse>
    {
        Task<IReadOnlyCollection<UserResponse>> GetResponsesByExamAsync(Guid examId);
        Task<bool> DeleteByQuestionIdAsync(Guid questionId);
        Task<bool> DeleteByExamIdAsync(Guid examId);
        //Task<IReadOnlyCollection<UserResponse>> GetResponsesByUserAsync(Guid userId); // Added for user-specific responses
        Task<double> CalculateExamScoreAsync(Guid examId);
        Task<bool> SubmitResponsesAsync(IEnumerable<UserResponse> responses);
        //Task<bool> HasUserTakenExamAsync(Guid userId, Guid examId); // Added for exam attempt check
    }
}