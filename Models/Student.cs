using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NupatDashboardProject.Models
{
	public class Student
	{
        public string Id { get; set; }
		public string FullName { get; set; }		
		public string? Cohort { get; set; }
		public string Course { get; set; }

	}
}
