using AtiExamSite.Models.ViewModels.Option;

namespace AtiExamSite.Models.ViewModels.Question
{
    public class QuestionDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DifficultyLevel { get; set; }
        public byte CorrectAnswer { get; set; }
        public List<OptionViewModel> Options { get; set; }
    }
}
