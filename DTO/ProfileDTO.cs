using System.ComponentModel.DataAnnotations;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.DTO
{
	public class ProfileDTO
	{
		public string? FullName { get; set; }
		public string? CourseOfInterest { get; set; }
        public string Bios { get; set; }
        public string? EmailAddress { get; set; }
		public string? PhoneNumber { get; set; }
		public string? HomeAddress { get; set; }
		public string SocialMediaAccounts { get; set; }
		public string IndustryInterests { get; set;}



	}
}
