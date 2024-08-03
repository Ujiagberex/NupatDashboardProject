using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.DTO
{
	public class RegisterStudentDTO
	{
		[Required]
		[EmailAddress]
		[StringLength(50)]
		public string Email { get; set; }
		[Required]
		[StringLength(100)]
		public string FullName { get; set; }
		[Required]
		[StringLength(100)]
		public string Course { get; set; }
		[Required]
		public string Cohorts { get; set; }
		[Required]
		public bool IsStudent { get; internal set; }
	}
}
