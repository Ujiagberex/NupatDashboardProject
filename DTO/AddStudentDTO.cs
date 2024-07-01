using System.ComponentModel.DataAnnotations.Schema;

namespace NupatDashboardProject.DTO
{
	public class AddStudentDTO : UserDTO
	{

		[ForeignKey("CourseId")]
        public Guid CourseId { get; set; }
		public Guid CohortId { get; set; }
	}
	public class ShowStudentDTO : UserDTO
	{

	}
}
