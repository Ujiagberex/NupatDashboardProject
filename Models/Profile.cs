using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
	public class Profile
    {
        [Key]
        public Guid ProfileId { get; set; }
        public string? FullName { get; set; }
        public string? EmailAddress { get; set; }
		[StringLength(11)]
		public string? PhoneNumber { get; set; }
        public string? HomeAddress { get; set; }
		public string? Bios { get; set; }
		public string? CourseOfInterest { get; set; }
		public List<IndustryInterest> IndustryInterests { get; set; }
		public List<SocialMediaAccount> SocialMediaAccounts { get; set; }


	}
}
