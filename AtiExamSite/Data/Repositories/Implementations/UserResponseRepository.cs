using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
namespace AtiExamSite.Data.Repositories.Implementations
{
    public class UserResponseRepository : RepositoryBase<UserResponse>, IUserResponseRepository
    {
        #region [- Ctor() -]
        public UserResponseRepository(ProjectDbContext context) : base(context)
        {

        }
        #endregion

        #region [- GetResponsesByExamAsync() -]
        public async Task<IReadOnlyCollection<UserResponse>> GetResponsesByExamAsync(Guid examId)
        {
            if (examId == Guid.Empty)
                throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

            return await _dbContext.UserResponses
                .Where(ur => ur.ExamId == examId)
                .ToListAsync();
        }
        #endregion

        #region [- SubmitResponsesAsync() -]
        public async Task<bool> SubmitResponsesAsync(IEnumerable<UserResponse> responses)
        {
            if (responses == null || !responses.Any())
                return false;

            await _dbContext.UserResponses.AddRangeAsync(responses);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region [- CalculateExamScoreAsync() -]
        public async Task<double> CalculateExamScoreAsync(Guid examId)
        {
            if (examId == Guid.Empty)
                throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

            // Get all user responses for this exam
            var responses = await _dbContext.UserResponses
                .Where(ur => ur.ExamId == examId && ur.SelectedOptionId != null && ur.SelectedOptionId != Guid.Empty)
                .ToListAsync();

            if (responses == null || responses.Count == 0)
                return 0.0;

            var selectedOptionIds = responses
                .Select(r => r.SelectedOptionId)
                .Distinct()
                .ToList();

            // Load only the selected options and check which are correct
            var selectedOptions = await _dbContext.Options
                .Where(o => selectedOptionIds.Contains(o.Id))
                .ToDictionaryAsync(o => o.Id, o => o.IsCorrect);

            int correctCount = responses.Count(r =>
                r.SelectedOptionId != null &&
                selectedOptions.TryGetValue(r.SelectedOptionId, out bool isCorrect) &&
                isCorrect);

            // If no valid responses were submitted, score is 0
            if (responses.Count == 0)
                return 0.0;

            double percentage = (double)correctCount / responses.Count * 100;
            return Math.Round(percentage, 2);
        }


        #endregion

        #region [- DeleteByQuestionIdAsync() -]
        public async Task<bool> DeleteByQuestionIdAsync(Guid questionId)
        {
            var responses = await _dbContext.UserResponses
                .Where(ur => ur.QuestionId == questionId)
                .ToListAsync();

            if (responses.Any())
            {
                _dbContext.UserResponses.RemoveRange(responses);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }

        #endregion

        //related to user(deleted from project and db)
        #region [- HasUserTakenExamAsync() -]
        //public async Task<bool> HasUserTakenExamAsync(Guid userId, Guid examId)
        //{
        //    if (examId == Guid.Empty)
        //        throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

        //    return await _dbContext.UserResponses
        //        .AnyAsync(ur => ur.UserId == userId && ur.ExamId == examId);
        //} 
        #endregion

        #region [- GetResponsesByUserAsync() -]
        //public async Task<IReadOnlyCollection<UserResponse>> GetResponsesByUserAsync(Guid userId)
        //{
        //    // If userId == Guid.Empty, return anonymous user responses (those with UserId == Guid.Empty)
        //    return await _dbContext.UserResponses
        //        .Where(ur => ur.UserId == userId)
        //        .ToListAsync();
        //} 
        #endregion
    }
}
