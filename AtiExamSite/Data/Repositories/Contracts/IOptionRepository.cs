using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IOptionRepository : IRepositoryBase<Option>
    {
        Task<IEnumerable<Option>> GetOptionsByQuestionAsync(Guid questionId);
        Task<Option?> GetCorrectOptionForQuestionAsync(Guid questionId);
        Task<bool> IsCorrectOptionAsync(Guid optionId);
        Task UpdateOptionAsync(Option updatedOption);
    }
}