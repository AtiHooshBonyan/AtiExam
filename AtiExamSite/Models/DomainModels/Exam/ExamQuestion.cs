namespace AtiExamSite.Models.DomainModels.Exam
{
    public class ExamQuestion
    {
        public string Id { get; set; }
        public string ExamId { get; set; }
        public string QuestionId { get; set; }


        // Navigation Properties
        public Exam Exam { get; set; }
        public Question Question { get; set; }
    }
}
