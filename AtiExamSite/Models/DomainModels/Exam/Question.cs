using Microsoft.AspNetCore.Hosting.Server;

namespace AtiExamSite.Models.DomainModels.Exam
{
    public class Question
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string DifficultyLevel { get; set; }


        // 1-to-Many: A Question has multiple QuestionOption
        public ICollection<QuestionOption> QuestionOptions { get; set; }

        // 1-to-Many: A Question can have multiple UserResponses
        public ICollection<UserResponse> UserResponses { get; set; }

        // Many-to-Many: A Question can belong to multiple Exams (via ExamQuestion)
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}