using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.RepositoriesImplementation
{
    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
        public ExamRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }

        #region [- GetAllExamsAsync() -]
        public async Task<IEnumerable<Exam>> GetAllWithQuestionsAsync()
        {
            return await _dbContext.Exams
                .Include(e => e.Questions)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region [- GetExamAsync() -]
        public async Task<Exam> GetWithQuestionsAsync(Guid id)
        {
            return await _dbContext.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == id);
        } 
        #endregion

        #region [- GetRandomQuestionsAsync() -]
        public async Task<IEnumerable<Question>> GetRandomQuestionsAsync(Guid examId, int count)
        {
            return await _dbContext.Questions
                .Where(q => q.ExamId == examId)
                .Include(q => q.AnswerOptions)
                .OrderBy(q => Guid.NewGuid()) // Randomize ordering
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        } 
        #endregion
    }
}

