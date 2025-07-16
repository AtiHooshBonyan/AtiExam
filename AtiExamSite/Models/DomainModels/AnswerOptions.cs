
namespace AtiExamSite.Models.DomainModels
{
    public class AnswerOption
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Question Question { get; set; }
    }
}
