using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Services.Contracts
{
    public interface IOptionService
    {
        Task<IEnumerable<Option>> GetAllOptionsAsync();
        Task<Option?> GetByIdAsync(Guid id);
        Task<bool> CreateOptionAsync(Option option);
        Task<IEnumerable<Option>> GetOptionsByQuestionAsync(Guid questionId);
        Task<Option?> GetCorrectOptionForQuestionAsync(Guid questionId);
        Task<bool> IsCorrectOptionAsync(Guid optionId);
        Task<bool> UpdateOptionAsync(Option option);
        Task<bool> DeleteOptionAsync(Guid optionId);
    }
}