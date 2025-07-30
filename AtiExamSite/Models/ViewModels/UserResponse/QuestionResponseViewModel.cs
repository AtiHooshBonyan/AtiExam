namespace AtiExamSite.Models.ViewModels.UserResponse
{
    public class QuestionResponseViewModel
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<OptionResponseViewModel> Options { get; set; }
        public Guid SelectedOptionId { get; set; }
    }
}
