using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IExamRepository : IRepositoryBase<Exam>
    {
        Task<IReadOnlyCollection<Exam>> GetExamsByUserAsync(Guid userId);
        Task<bool> ExistsAsync(string title, Guid? excludeExamId = null);
        Task<Exam?> GetExamWithQuestionsAsync(Guid examId);
        Task<IReadOnlyCollection<Question>> GetRandomQuestionsAsync(int count);
    }
}