using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Data.Repositories.Contracts
{
    public interface IOptionRepository : IRepositoryBase<Option>
    {
        Task<IEnumerable<Option>> GetOptionsByQuestionAsync(string questionId);
        Task<Option?> GetCorrectOptionForQuestionAsync(string questionId);
        Task<bool> IsCorrectOptionAsync(string optionId);
    }
}