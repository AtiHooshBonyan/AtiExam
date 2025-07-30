namespace AtiExamSite.Models.ViewModels.Question
{
    public class OptionCheckboxViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCorrect { get; set; }
    }
}