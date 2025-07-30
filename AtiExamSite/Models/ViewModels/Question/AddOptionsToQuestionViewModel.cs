namespace AtiExamSite.Models.ViewModels.Question
{
    public class AddOptionsToQuestionViewModel
    {
        public Guid QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public List<OptionCheckboxViewModel> AvailableOptions { get; set; }
    }
}