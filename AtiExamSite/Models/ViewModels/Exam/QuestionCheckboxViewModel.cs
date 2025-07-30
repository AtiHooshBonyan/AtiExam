namespace AtiExamSite.Models.ViewModels.Exam
{
    public class QuestionCheckboxViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DifficultyLevel { get; set; }
        public bool IsSelected { get; set; }
    }
}
