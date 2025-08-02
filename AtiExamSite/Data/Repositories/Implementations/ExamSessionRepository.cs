using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.Implementations
{
    public class ExamSessionRepository : IExamSessionRepository
    {
        private readonly ProjectDbContext _dbContext;

        public ExamSessionRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExamSession> GetByUserAndExamAsync(Guid examId)
        {
            return await _dbContext.ExamSessions
                .FirstOrDefaultAsync(s =>  s.ExamId == examId);
        }

        public async Task AddAsync(ExamSession session)
        {
            await _dbContext.ExamSessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExamSession session)
        {
            _dbContext.ExamSessions.Update(session);
            await _dbContext.SaveChangesAsync();
        }
    }

}
