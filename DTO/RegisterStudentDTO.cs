using System.ComponentModel.DataAnnotations;
using NupatDashboardProject.Models;

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
		public Guid CourseId { get; set; }
		public string CohortId { get; set; }
        [Required]
		public bool IsStudent { get; internal set; }
	}
}
