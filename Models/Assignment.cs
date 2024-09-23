namespace NupatDashboardProject.Models
{
	public class Assignment
	{
		
		public Guid AssignmentId { get; set; }
		public byte[] FileData { get; set; }
		public DateTime DateUploaded { get; set; }
		public DateTime DueDate { get; set; }
		public string FilePath { get; set; }
		public string? Status { get; set; }

        // Relationship: Assignment belongs to one Course
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        // Relationship: Assignment has many SubmittedAssignments
        public ICollection<SubmitAssignment> SubmittedAssignments { get; set; }
    }
}
