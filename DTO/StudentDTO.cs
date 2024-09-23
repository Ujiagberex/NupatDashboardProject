using System.ComponentModel.DataAnnotations;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.DTO
{
	public class StudentDTO
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string FullName { get; set; }		
		public Guid CourseId { get; set; }
		public string CohortId { get; set; }
	}
}
