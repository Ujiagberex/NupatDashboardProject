using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NupatDashboardProject.Models
{
	public class Student : User
	{
		[Key]
		public Guid StudentId { get; set; }
		[ForeignKey(nameof(CohortId))]
		public Guid CohortId { get; set; }
        
		[ForeignKey("CourseId")]
		public Guid CourseId { get; set; }
		public Course Course { get; set; }
    }
}
