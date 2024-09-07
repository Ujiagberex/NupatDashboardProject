using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NupatDashboardProject.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
		
		[StringLength(50)]
		public string? Cohort { get; set; }
        
		[Required]
		[StringLength(50)]
		public string Course { get; set; }
		public bool IsPasswordChanged { get; set; }

		public ICollection<SubmitAssignment> SubmittedAssignments { get; set; }
	}
}
