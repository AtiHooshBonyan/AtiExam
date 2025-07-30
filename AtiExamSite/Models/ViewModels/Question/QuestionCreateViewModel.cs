namespace AtiExamSite.Models.ViewModels.Question
{
    public class QuestionCreateViewModel
    {
        public string Title { get; set; }
        public string DifficultyLevel { get; set; }
        public byte CorrectAnswer { get; set; }
    }
}
