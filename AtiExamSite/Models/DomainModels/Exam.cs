
namespace AtiExamSite.Models.DomainModels{
    public class Exam
    {
        public Guid ExamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? TimeLimitMinutes { get; set; } // null == no time limit
        public int QuestionsToShow { get; set; }
        public int PassingScore { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}