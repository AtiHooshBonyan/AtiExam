using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.RepositoriesImplementation
{
    public class ExamSessionRepository : BaseRepository<ExamSession>, IExamSessionRepository
    {
        #region [- Ctor() -]
        public ExamSessionRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region [- GetSessionsByUserAsync() -]
        public async Task<IEnumerable<ExamSession>> GetSessionsByUserAsync(string userId)
        {
            return await _dbContext.ExamSessions
                .Where(s => s.UserId == userId)
                .Include(s => s.Exam)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region [- GetSessionWithDetailsAsync() -]
        public async Task<ExamSession> GetSessionWithDetailsAsync(Guid sessionId)
        {
            return await _dbContext.ExamSessions
                .Include(s => s.Exam)
                .Include(s => s.UserResponses)
                    .ThenInclude(r => r.Question)
                .Include(s => s.UserResponses)
                    .ThenInclude(r => r.AnswerOption)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }
        #endregion

        #region [- GetSessionsByExamAsync() -]
        public async Task<IEnumerable<ExamSession>> GetSessionsByExamAsync(Guid examId)
        {
            return await _dbContext.ExamSessions
                .Where(s => s.ExamId == examId)
                .Include(s => s.UserResponses)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region [- StartNewSessionAsync() -]
        public async Task<ExamSession> StartNewSessionAsync(Guid examId, string userId)
        {
            var exam = await _dbContext.Exams.FindAsync(examId);
            if (exam == null) throw new ArgumentException("Exam not found");

            var session = new ExamSession
            {
                SessionId = Guid.NewGuid(),
                ExamId = examId,
                UserId = userId,
                StartTime = DateTime.UtcNow,
                Status = "InProgress"
            };

            await _dbContext.ExamSessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }
        #endregion

        #region [- CompleteSessionAsync() -]
        public async Task<bool> CompleteSessionAsync(Guid sessionId)
        {
            var session = await _dbContext.ExamSessions.FindAsync(sessionId);
            if (session == null) return false;

            session.EndTime = DateTime.UtcNow;
            session.Status = "Completed";

            // Delegate score calculation to UserResponseRepository
            var userResponseRepo = new UserResponseRepository(_dbContext);
            session.Score = await userResponseRepo.CalculateSessionScoreAsync(sessionId);

            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- CheckAndEndExpiredSessionsAsync() -]
        public async Task<bool> CheckAndEndExpiredSessionsAsync()
        {
            var now = DateTime.UtcNow;
            var expiredSessions = await _dbContext.ExamSessions
                .Include(s => s.Exam)
                .Where(s => s.Status == "InProgress" &&
                           s.Exam.TimeLimitMinutes != null &&
                           s.StartTime.AddMinutes(s.Exam.TimeLimitMinutes.Value) < now)
                .ToListAsync();

            var userResponseRepo = new UserResponseRepository(_dbContext);

            foreach (var session in expiredSessions)
            {
                session.EndTime = now;
                session.Status = "AutoCompleted";

                // Delegate score calculation to UserResponseRepository
                session.Score = await userResponseRepo.CalculateSessionScoreAsync(session.SessionId);
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- IsSessionActiveAsync() -]
        public async Task<bool> IsSessionActiveAsync(Guid sessionId)
        {
            return await _dbContext.ExamSessions
                .AnyAsync(s => s.SessionId == sessionId &&
                              s.Status == "InProgress" &&
                              (s.Exam.TimeLimitMinutes == null ||
                               s.StartTime.AddMinutes(s.Exam.TimeLimitMinutes.Value) > DateTime.UtcNow));
        }
        #endregion

        #region [- HasUserTakenExamAsync() -]
        public async Task<bool> HasUserTakenExamAsync(string userId, Guid examId)
        {
            return await _dbContext.ExamSessions
                .AnyAsync(s => s.UserId == userId &&
                              s.ExamId == examId &&
                              s.Status != "InProgress");
        }
        #endregion
    }
}