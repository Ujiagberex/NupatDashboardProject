using System.ComponentModel.DataAnnotations;
using Humanizer;

namespace NupatDashboardProject.Models
{
	public class Facilitator : ApplicationUser
    {
        // A facilitator can teach multiple courses(One-to-Many)
        public ICollection<Course> Courses { get; set; }

    }
}
