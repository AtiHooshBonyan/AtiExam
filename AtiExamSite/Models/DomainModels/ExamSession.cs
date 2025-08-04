namespace AtiExamSite.Models.DomainModels
{
    public class ExamSession
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public DateTime StartTime { get; set; }

        // NEW:
        public DateTime? EndTime { get; set; }
        public bool IsCompleted => EndTime.HasValue;
    }


}
