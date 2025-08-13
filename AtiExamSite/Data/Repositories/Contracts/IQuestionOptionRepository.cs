using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IQuestionOptionRepository : IRepositoryBase<QuestionOption>
    {
        Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(string questionId);
        Task<bool> AddOptionsToQuestionAsync(string questionId, IEnumerable<string> optionIds);
        Task<bool> RemoveOptionsFromQuestionAsync(string questionId, IEnumerable<string> optionIds); 
    }
}