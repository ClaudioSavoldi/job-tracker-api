using Microsoft.AspNetCore.Identity;

namespace JobTracker.API.Models
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public ICollection<JobApplication> JobApplications { get; set; }= new List<JobApplication>();
    }
}
