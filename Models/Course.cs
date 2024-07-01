using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
	public class Course
	{
		[Key]
		public Guid CourseId { get; set; }
		public string? Title { get; set; }
		[ForeignKey("FacilitatorId")]
        public Guid FacilitatorId { get; set; }
		public Facilitator Facilitator { get; set; }
		public ICollection<Student> Students { get; set; }

	}
}
