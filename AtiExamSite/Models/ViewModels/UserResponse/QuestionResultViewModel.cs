namespace AtiExamSite.Models.ViewModels.UserResponse
{
    public class QuestionResultViewModel
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public Guid SelectedOptionId { get; set; }
        public string SelectedOptionText { get; set; }
        public bool IsCorrect { get; set; }
        public Guid CorrectOptionId { get; set; }
        public string CorrectOptionText { get; set; }
    }
}
