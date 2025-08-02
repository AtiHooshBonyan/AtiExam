
using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Data.Repositories.Implementations;
using AtiExamSite.Models.DomainModels;
using AtiExamSite.Services.Contracts;

namespace AtiExamSite.Services.Implementations
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IUserResponseRepository _userResponseRepository;

        #region [- Ctor() -]
        public ExamService(IExamRepository examRepository, IUserResponseRepository userResponseRepository)
        {
            _examRepository = examRepository;
            _userResponseRepository=userResponseRepository;
        }
        #endregion

        #region [- GetExamsByUserAsync() -]
        public async Task<IReadOnlyCollection<Exam>> GetExamsByUserAsync(Guid userId)
        {
            var exams = await _examRepository.GetExamsByUserAsync(userId);
            return exams?.ToList().AsReadOnly() ?? new List<Exam>().AsReadOnly();
        }
        #endregion

        #region [- ExamExistsAsync() -]
        public async Task<bool> ExamExistsAsync(string title, Guid? excludeExamId = null)
        {
            return await _examRepository.ExistsAsync(title, excludeExamId);
        }
        #endregion

        #region [- GetExamWithQuestionsAsync() -]
        public async Task<Exam?> GetExamWithQuestionsAsync(Guid examId)
        {
            return await _examRepository.GetExamWithQuestionsAsync(examId);
        }
        #endregion

        #region [- GetRandomQuestionsAsync() -]
        public async Task<IReadOnlyCollection<Question>> GetRandomQuestionsAsync(int count)
        {
            if (count <= 0) throw new ArgumentException("Count must be greater than zero", nameof(count));
            return await _examRepository.GetRandomQuestionsAsync(count);
        }
        #endregion

  

        #region [- GetByIdAsync() -]
        public async Task<Exam?> GetByIdAsync(Guid id)
        {
            return await _examRepository.GetByIdAsync(id);
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<IReadOnlyCollection<Exam>> GetAllAsync()
        {
            var exams = await _examRepository.GetAllAsync();
            return exams?.ToList().AsReadOnly() ?? new List<Exam>().AsReadOnly();
        }
        #endregion

        #region [- AddAsync() -]
        public async Task<bool> AddAsync(Exam entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (await _examRepository.ExistsAsync(entity.Title))
                throw new InvalidOperationException($"Exam with title '{entity.Title}' already exists");

            await _examRepository.AddAsync(entity);
            return await _examRepository.SaveChangesAsync();
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task<bool> UpdateAsync(Exam entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (await _examRepository.ExistsAsync(entity.Title, entity.Id))
                throw new InvalidOperationException($"Exam with title '{entity.Title}' already exists");

            await _examRepository.UpdateAsync(entity);
            return await _examRepository.SaveChangesAsync();
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<bool> DeleteAsync(Guid id)
        {
            var exam = await _examRepository.GetByIdAsync(id);
            if (exam == null) return false;
            // Delete related UserResponses first to avoid FK conflict
            await _userResponseRepository.DeleteByExamIdAsync(id);
            await _examRepository.DeleteAsync(exam);
            return await _examRepository.SaveChangesAsync();
        } 
        #endregion
    }
}