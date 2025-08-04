using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Services.Contracts
{
    public interface IExamService
    {
        Task<bool> ExamExistsAsync(string title, Guid? excludeExamId = null);
        Task<Exam?> GetExamWithQuestionsAsync(Guid examId);
        Task<IReadOnlyCollection<Question>> GetRandomQuestionsAsync(int count);

        // CRUD operations
        Task<Exam?> GetByIdAsync(Guid id);
        Task<IReadOnlyCollection<Exam>> GetAllAsync();
        Task<bool> AddAsync(Exam entity);
        Task<bool> UpdateAsync(Exam entity);
        Task<bool> DeleteAsync(Guid id);
    }
}