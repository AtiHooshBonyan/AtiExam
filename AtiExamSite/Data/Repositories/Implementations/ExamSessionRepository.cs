using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class ExamSessionRepository : IExamSessionRepository
    {
        private readonly ProjectDbContext _dbContext;

        #region [- Ctor() -]
        public ExamSessionRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region [- GetByUserAndExamAsync() -]
        public async Task<ExamSession> GetByUserAndExamAsync(string examId)
        {
            return await _dbContext.ExamSessions
                .FirstOrDefaultAsync(s => s.ExamId == examId);
        }
        #endregion

        #region [- AddAsync() -]
        public async Task AddAsync(ExamSession session)
        {
            await _dbContext.ExamSessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task UpdateAsync(ExamSession session)
        {
            _dbContext.ExamSessions.Update(session);
            await _dbContext.SaveChangesAsync();
        } 
        #endregion
    }

}
