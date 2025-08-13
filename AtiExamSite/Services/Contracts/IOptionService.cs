using AtiExamSite.Models.DomainModels.Exam;

namespace AtiExamSite.Services.Contracts
{
    public interface IOptionService
    {
        Task<IEnumerable<Option>> GetAllOptionsAsync();
        Task<Option?> GetByIdAsync(string id);
        Task<bool> CreateOptionAsync(Option option);
        Task<IEnumerable<Option>> GetOptionsByQuestionAsync(string questionId);
        Task<Option?> GetCorrectOptionForQuestionAsync(string questionId);
        Task<bool> IsCorrectOptionAsync(string optionId);
        Task<bool> UpdateOptionAsync(Option option);
        Task<bool> DeleteOptionAsync(string optionId);
    }
}