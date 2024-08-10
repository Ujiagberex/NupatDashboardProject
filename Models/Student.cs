using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NupatDashboardProject.Models
{
	public class Student : User
	{
		[Key]
		public Guid StudentId { get; set; }
		public Course Course { get; set; }
    }
}
