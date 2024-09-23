namespace NupatDashboardProject.Models
{
    public class Student : ApplicationUser
	{

        public string CohortId { get; set; }
        public Cohort Cohort { get; set; }
        // A student can enroll in many courses (Many-to-Many)
        public ICollection<Course> Courses { get; set; }
        public ICollection<SubmitAssignment> SubmittedAssignments { get; set; }

    }
}
