using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IQuestionOptionService
    {
        Task<bool> AddOptionsToQuestionAsync(string questionId, IEnumerable<string> optionIds);
        Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(string questionId);
        Task<bool> RemoveOptionsFromQuestionAsync(string questionId, IEnumerable<string> optionIds);
    }
}