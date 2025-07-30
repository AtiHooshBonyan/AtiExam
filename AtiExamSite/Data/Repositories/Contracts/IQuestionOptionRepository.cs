using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IQuestionOptionRepository : IRepositoryBase<QuestionOption>
    {
        Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(Guid questionId);
        Task<bool> AddOptionsToQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds);
        Task<bool> RemoveOptionsFromQuestionAsync(Guid questionId, IEnumerable<Guid> optionIds); 
    }
}