namespace AtiExamSite.Models.DomainModels.Exam
{
    public class UserResponse
    {
        public Guid ExamId { get; set; }
        public Guid SelectedOptionId { get; set; }

        public Guid QuestionId { get; set; }

        public Question Question { get; set; }

        public Exam Exam { get; set; }
    }

}
