using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;

namespace AtiExamSite.Services.Implementations
{
    public class UserResponseService : IUserResponseService
    {
        private readonly IUserResponseRepository _userResponseRepository;

        #region [- Ctor() -]
        public UserResponseService(IUserResponseRepository userResponseRepository)
        {
            _userResponseRepository = userResponseRepository ?? throw new ArgumentNullException(nameof(userResponseRepository));
        }
        #endregion

        #region [- SubmitResponsesAsync() -]
        public async Task<bool> SubmitResponsesAsync(IEnumerable<UserResponse> responses)
        {
            if (responses == null || !responses.Any())
                return false;

            // Validate each response has required fields
            foreach (var response in responses)
            {
                if (response.ExamId == string.Empty || response.QuestionId == string.Empty)
                    return false;

                response.Id = Guid.NewGuid().ToString();
                // No validation for UserId to allow anonymous
            }

            return await _userResponseRepository.SubmitResponsesAsync(responses);
        }
        #endregion

        #region [- GetByExamAsync() -]
        public async Task<IReadOnlyCollection<UserResponse>> GetByExamAsync(string examId)
        {
            if (examId == string.Empty)
                throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

            return await _userResponseRepository.GetResponsesByExamAsync(examId);
        }
        #endregion

        #region [- GetByUserAsync() -]
        //public async Task<IReadOnlyCollection<UserResponse>> GetByUserAsync(Guid userId)
        //{
        //    // Allow Guid.Empty userId for anonymous users, so no exception thrown here
        //    return await _userResponseRepository.GetResponsesByUserAsync(userId);
        //}
        #endregion

        #region [- CalculateExamScoreAsync() -]
        public async Task<(double Score, bool Passed)> CalculateExamScoreAsync(string examId)
        {
            if (string.IsNullOrWhiteSpace(examId))

                throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

            return await _userResponseRepository.CalculateExamScoreAsync(examId);
        }
        #endregion

        #region [- HasUserTakenExamAsync() -]
        //public async Task<bool> HasUserTakenExamAsync(Guid userId, Guid examId)
        //{
        //    // Allow Guid.Empty userId for anonymous users, so no exception thrown here

        //    if (examId == Guid.Empty)
        //        throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

        //    return await _userResponseRepository.HasUserTakenExamAsync(userId, examId);
        //}
        #endregion
    }
}
