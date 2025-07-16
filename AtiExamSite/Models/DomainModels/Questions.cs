using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Models.DomainModels
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public Guid ExamId { get; set; }
        public string Text { get; set; }
        public string DifficultyLevel { get; set; }
        public string Category { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
    }
}