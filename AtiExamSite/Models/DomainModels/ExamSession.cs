using AtiExamSite.Models.DomainModels;

namespace AtiExamSite.Models.DomainModels
{
    public class ExamSession
    {
        public Guid SessionId { get; set; }
        public Guid ExamId { get; set; }
        public string UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public int? Score { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ICollection<UserResponse> UserResponses { get; set; }
    }
}
