using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;

namespace AtiExamSite.Services.Implementations
{
    public class ExamQuestionService : IExamQuestionService
    {
        private readonly IExamQuestionRepository _examQuestionRepository;

        #region [- Ctor() -]
        public ExamQuestionService(IExamQuestionRepository examQuestionRepository)
        {
            _examQuestionRepository = examQuestionRepository ?? throw new ArgumentNullException(nameof(examQuestionRepository));
        }
        #endregion

        #region [- AddQuestionsToExamAsync() -]
        public async Task<bool> AddQuestionsToExamAsync(Guid examId, IEnumerable<Guid> questionIds)
        {
            if (examId == Guid.Empty) throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
            if (questionIds == null || !questionIds.Any())
                throw new ArgumentException("Question IDs cannot be null or empty", nameof(questionIds));

            return await _examQuestionRepository.AddQuestionsToExamAsync(examId, questionIds);
        }
        #endregion

        #region [- GetExamQuestionsAsync() -]
        public async Task<IEnumerable<Question>> GetExamQuestionsAsync(Guid examId)
        {
            if (examId == Guid.Empty) throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

            var examQuestions = await _examQuestionRepository.GetByExamIdAsync(examId);
            return examQuestions?.Select(eq => eq.Question).ToList().AsReadOnly()
                   ?? new List<Question>().AsReadOnly();
        }
        #endregion

        #region [- ExistsAsync() -]
        public async Task<bool> ExistsAsync(Guid examId, Guid questionId)
        {
            if (examId == Guid.Empty) throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
            if (questionId == Guid.Empty) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));

            return await _examQuestionRepository.ExistsAsync(examId, questionId);
        }
        #endregion

        #region [- CountQuestionsInExamAsync() -]
        public async Task<int> CountQuestionsInExamAsync(Guid examId)
        {
            if (examId == Guid.Empty) throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
            return await _examQuestionRepository.CountQuestionsInExamAsync(examId);
        }
        #endregion
    }
}