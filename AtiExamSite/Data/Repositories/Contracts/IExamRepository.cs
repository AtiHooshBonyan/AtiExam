using AtiExamSite.Models.DomainModels;
using AtiExamSite.Data.Repositories.Contracts;

public interface IExamRepository : IRepositoryBase<Exam>
{
    Task<IEnumerable<Exam>> GetAllWithQuestionsAsync();
    Task<Exam> GetWithQuestionsAsync(Guid id);
    Task<IEnumerable<Question>> GetRandomQuestionsAsync(Guid examId, int count);
}
