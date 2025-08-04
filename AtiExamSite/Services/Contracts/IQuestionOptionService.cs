using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IQuestionOptionService
    {
        Task<bool> AddOptionsToQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds);
        Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(Guid questionId);
        Task<bool> RemoveOptionsFromQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds);
    }
}