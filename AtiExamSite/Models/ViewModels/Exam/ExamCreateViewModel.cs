namespace AtiExamSite.Models.ViewModels.Exam
{
    public class ExamCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public int QuestionCount { get; set; }
        public int RequiredQuestion { get; set; }
        public int PassingScore { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
