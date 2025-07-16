using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Models.DomainModels
{
    public class UserResponse
    {
        public Guid ResponseId { get; set; }
        public Guid SessionId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? AnswerId { get; set; } // user can leave the question with no answer
        public bool? IsCorrect { get; set; }
        public DateTime ResponseTime { get; set; }

        public virtual ExamSession ExamSession { get; set; }
        public virtual Question Question { get; set; }
        public virtual AnswerOption AnswerOption { get; set; }
    }
}
