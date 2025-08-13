using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IExamQuestionService
    {
        //Task<List<Question>> GetRandomQuestionsForExamAsync(Guid examId, int count);

        Task<bool> AddQuestionsToExamAsync(string examId, IEnumerable<string> questionIds);
        Task<IEnumerable<Question>> GetExamQuestionsAsync(string examId);
        Task<bool> ExistsAsync(string examId, string questionId);
        Task<int> CountQuestionsInExamAsync(string examId);
    }
}