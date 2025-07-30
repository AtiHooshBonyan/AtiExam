namespace AtiExamSite.Models.DomainModels
{
    public class ExamQuestion
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }


        // Navigation Properties
        public Exam Exam { get; set; }
        public Question Question { get; set; }
    }
}
