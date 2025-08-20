using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IExamQuestionService
    {
        Task<bool> AddQuestionsToExamAsync(string examId, IEnumerable<string> questionIds);
        Task<IEnumerable<Question>> GetExamQuestionsAsync(string examId);
        Task<int> CountQuestionsInExamAsync(string examId);
    }
}