using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.DTO
{
	public class StudentDTO
	{
		public string Id { get; set; }
		public string Email { get; set; }	
		public string FullName { get; set; }		
		public string Course { get; set; }
		public string Cohort { get; set; }
	}
}
