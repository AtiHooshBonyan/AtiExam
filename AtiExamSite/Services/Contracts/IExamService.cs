using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IExamService
    {
        Task<Exam?> GetExamWithQuestionsAsync(string examId);
        Task<IReadOnlyCollection<Question>> GetRandomQuestionsAsync(int count);

        // CRUD operations
        Task<Exam?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Exam>> GetAllAsync();
        Task<bool> AddAsync(Exam entity);
        Task<bool> UpdateAsync(Exam entity);
        Task<bool> DeleteAsync(string id);
    }
}