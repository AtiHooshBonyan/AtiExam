
using System.ComponentModel.DataAnnotations.Schema;

namespace AtiExamSite.Models.DomainModels
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        
        //public Guid UserId { get; set; }
        public Guid SelectedOptionId { get; set; }

        public Guid QuestionId { get; set; }

        public Question Question { get; set; }

        public Exam Exam { get; set; }

    }

}
