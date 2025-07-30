namespace AtiExamSite.Models.ViewModels.UserResponse
{
    public class ExamResultViewModel
    {
        public Guid ExamId { get; set; }
        public string ExamTitle { get; set; }
        public int Score { get; set; }
        public int PassingScore { get; set; }
        public bool HasPassed { get; set; }
        public List<QuestionResultViewModel> QuestionResults { get; set; }
    }
}
