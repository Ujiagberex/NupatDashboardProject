using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
    public class Course
	{
		[Key]
		public Guid CourseId { get; set; }
        [Required]
		public string Title { get; set; }

        // A course has one facilitator
        public string FacilitatorId { get; set; }
        public Facilitator Facilitator { get; set; }
        // A course has many assignments
        public ICollection<Assignment> Assignments { get; set; }

        // A course belongs to one cohort (One-to-Many)
        public string CohortId { get; set; } // Foreign key
        public Cohort Cohort { get; set; } // Navigation property

        // A course has many pieces of content
        public ICollection<Content> Contents { get; set; }

        // A course can have many students (Many-to-Many)
        public ICollection<Student> Students { get; set; }

    }
}
