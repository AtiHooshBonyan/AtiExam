using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IUserResponseRepository : IRepositoryBase<UserResponse>
    {
        Task<IReadOnlyCollection<UserResponse>> GetResponsesByExamAsync(string examId);
        Task<bool> DeleteByQuestionIdAsync(string questionId);
        Task<bool> DeleteByExamIdAsync(string examId);
        //Task<IReadOnlyCollection<UserResponse>> GetResponsesByUserAsync(Guid userId); // Added for user-specific responses
        Task<(double Score, bool Passed)> CalculateExamScoreAsync(string examId);
        Task<bool> SubmitResponsesAsync(IEnumerable<UserResponse> responses);
        //Task<bool> HasUserTakenExamAsync(Guid userId, Guid examId); // Added for exam attempt check
    }
}