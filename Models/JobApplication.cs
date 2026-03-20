using JobTracker.API.Models.Enums;

namespace JobTracker.API.Models
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } 
        public Guid UserId { get; set; } 
        public ApplicationUser? User { get; set; }

    }
}
