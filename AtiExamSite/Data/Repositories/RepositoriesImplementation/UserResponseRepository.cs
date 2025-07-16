using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AtiExamSite.Data.Repositories.RepositoriesImplementation
{
    public class UserResponseRepository : BaseRepository<UserResponse>, IUserResponseRepository
    {
        #region [- Ctor() -]
        public UserResponseRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        } 
        #endregion

        #region [- GetResponsesBySessionAsync() -]
        public async Task<IEnumerable<UserResponse>> GetResponsesBySessionAsync(Guid sessionId)
        {
            return await _dbContext.UserResponses
                .Where(r => r.SessionId == sessionId)
                .Include(r => r.Question)
                .Include(r => r.AnswerOption)
                .AsNoTracking()
                .ToListAsync();
        } 
        #endregion

        #region [- GetResponsesByQuestionAsync() -]
        public async Task<IEnumerable<UserResponse>> GetResponsesByQuestionAsync(Guid questionId)
        {
            return await _dbContext.UserResponses
                .Where(r => r.QuestionId == questionId)
                .Include(r => r.ExamSession)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region [- CalculateSessionScoreAsync() -]
        public async Task<int> CalculateSessionScoreAsync(Guid sessionId)
        {
            var correct = await GetCorrectAnswerCountAsync(sessionId);
            var total = await GetTotalQuestionCountAsync(sessionId);
            return total > 0 ? (int)Math.Round((double)correct / total * 100) : 0;
        }
        #endregion

        #region [- GetCorrectAnswerCountAsync() -]
        public async Task<int> GetCorrectAnswerCountAsync(Guid sessionId)
        {
            return await _dbContext.UserResponses
                .Where(r => r.SessionId == sessionId && r.IsCorrect == true)
                .CountAsync();
        }
        #endregion

        #region [- GetTotalQuestionCountAsync() -]
        public async Task<int> GetTotalQuestionCountAsync(Guid sessionId)
        {
            return await _dbContext.UserResponses
                .Where(r => r.SessionId == sessionId)
                .CountAsync();
        } 
        #endregion


    }
}