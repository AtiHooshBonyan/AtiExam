namespace AtiExamSite.Models.DomainModels.Exam
{
    public class ExamSession
    {
        public string Id { get; set; }
        public string ExamId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        public bool IsCompleted => EndTime.HasValue;
    }


}
