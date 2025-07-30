namespace AtiExamSite.Models.ViewModels.UserResponse
{
    public class UserResponseCreateViewModel
    {
        public Guid ExamId { get; set; }
        public Guid UserId { get; set; }
        public List<QuestionResponseViewModel> Questions { get; set; }
    }
}
