namespace AtiExamSite.Models.DomainModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public bool IsAdmin { get; set; } = false; // Admin flag

        // Navigation property for exam responses (to track exam results)
        public ICollection<UserResponse> UserResponses { get; set; }
    }
}
