using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IExamRepository : IRepositoryBase<Exam>
    {
        Task<bool> ExistsAsync(string title, string? excludeExamId = null);
        Task<Exam?> GetExamWithQuestionsAsync(string examId);
        Task<IReadOnlyCollection<Question>> GetRandomQuestionsAsync(int count);
    }
}