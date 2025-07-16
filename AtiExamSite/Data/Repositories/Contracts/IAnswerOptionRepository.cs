using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IAnswerOptionRepository : IRepositoryBase<AnswerOption>
    {
        Task<IEnumerable<AnswerOption>> GetAnswerOptionsByQuestionAsync(Guid questionId);
        Task<IEnumerable<AnswerOption>> GetCorrectAnswersForQuestionAsync(Guid questionId);
    }
}