using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;
using Microsoft.EntityFrameworkCore;

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
        public async Task<bool> AddQuestionsToExamAsync(string examId, IEnumerable<string> questionIds)
        {
            foreach (var qid in questionIds)
            {
                var eq = new ExamQuestion
                {
                    Id = Guid.NewGuid().ToString(), 
                    ExamId = examId,
                    QuestionId = qid
                };

                await _examQuestionRepository.AddAsync(eq);
            }

            await _examQuestionRepository.SaveChangesAsync();
            return true;
        }

        #endregion

        #region [- GetExamQuestionsAsync() -]
        public async Task<IEnumerable<Question>> GetExamQuestionsAsync(string examId)
        {
            if (examId == Guid.Empty.ToString()) throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

            var examQuestions = await _examQuestionRepository.GetByExamIdAsync(examId);
            return examQuestions?.Select(eq => eq.Question).ToList().AsReadOnly()
                   ?? new List<Question>().AsReadOnly();
        }
        #endregion

        #region [- ExistsAsync() -]
        public async Task<bool> ExistsAsync(string examId, string questionId)
        {
            if (examId == Guid.Empty.ToString()) throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
            if (questionId == Guid.Empty.ToString()) throw new ArgumentException("Question ID cannot be empty", nameof(questionId));

            return await _examQuestionRepository.ExistsAsync(examId, questionId);
        }
        #endregion

        #region [- CountQuestionsInExamAsync() -]
        public async Task<int> CountQuestionsInExamAsync(string examId)
        {
            if (examId == Guid.Empty.ToString()) throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
            return await _examQuestionRepository.CountQuestionsInExamAsync(examId);
        }
        #endregion
    }
}