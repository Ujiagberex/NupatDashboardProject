using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NupatDashboardProject.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
		public Guid CourseId { get; set; }
		public bool IsPasswordChanged { get; set; }

		
	}
}
