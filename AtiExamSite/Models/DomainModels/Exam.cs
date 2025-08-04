
namespace AtiExamSite.Models.DomainModels{
    public class Exam
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public int QuestionCount { get; set; }
        public int RequiredQuestion { get; set; }
        public double PassingScore { get; set; }
        public bool IsActive { get; set; } = true; 

        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<UserResponse> UserResponses { get; set; }
    }
}