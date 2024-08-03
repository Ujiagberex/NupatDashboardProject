using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NupatDashboardProject.Models
{
	public class FacilitatorAppUser : IdentityUser
	{
		public string FullName { get; set; }
		[Required]
		[StringLength(50)]
		public string Course { get; set; }
	}
}
