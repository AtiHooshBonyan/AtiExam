namespace AtiExamSite.Models.DomainModels.Exam
{
    public class QuestionOption

    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string OptionId { get; set; }

        public Question Question { get; set; }
        public Option Option { get; set; }
    }
}
