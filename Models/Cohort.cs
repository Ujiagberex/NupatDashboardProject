namespace NupatDashboardProject.Models
{
    public class Cohort
    {
        public string Id { get; set; }
        public string Name { get; set; }
        // A cohort can have many students
        public ICollection<Student> Students { get; set; }

        // Cohort has many courses (One-to-Many)
        public ICollection<Course> Courses { get; set; }
    }
}
