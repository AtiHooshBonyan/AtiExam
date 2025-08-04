using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IExamQuestionService
    {
        //Task<List<Question>> GetRandomQuestionsForExamAsync(Guid examId, int count);

        Task<bool> AddQuestionsToExamAsync(Guid examId, IEnumerable<Guid> questionIds);
        Task<IEnumerable<Question>> GetExamQuestionsAsync(Guid examId);
        Task<bool> ExistsAsync(Guid examId, Guid questionId);
        Task<int> CountQuestionsInExamAsync(Guid examId);
    }
}