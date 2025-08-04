namespace AtiExamSite.Models.DomainModels.Exam
{
    public class Option
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCorrect { get; set; }

        public ICollection<QuestionOption> QuestionOptions { get; set; }
    }
}
