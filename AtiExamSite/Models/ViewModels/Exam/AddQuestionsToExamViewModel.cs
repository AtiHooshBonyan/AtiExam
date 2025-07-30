namespace AtiExamSite.Models.ViewModels.Exam
{
    public class AddQuestionsToExamViewModel
    {
        public Guid ExamId { get; set; }
        public string ExamTitle { get; set; }
        public List<QuestionCheckboxViewModel> AvailableQuestions { get; set; }
    }
}

