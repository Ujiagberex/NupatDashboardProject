using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
	public class Course
	{
		[Key]
		public Guid CourseId { get; set; }
		public string? Title { get; set; }
        public string FacilitatorName { get; set; }
		public Facilitator Facilitator { get; set; }
		public ICollection<Student> Students { get; set; }

	}
}
