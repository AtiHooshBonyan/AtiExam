namespace AtiExamSite.Models.DomainModels.Exam
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string ExamId { get; set; }
        public string SelectedOptionId { get; set; }
        public string QuestionId { get; set; }

        public Question Question { get; set; }

        public Exam Exam { get; set; }
    }

}
