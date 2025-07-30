
namespace AtiExamSite.Models.DomainModels
{
    public class QuestionOption

    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Guid OptionId { get; set; }

        public Question Question { get; set; }
        public Option Option { get; set; }
    }
}
