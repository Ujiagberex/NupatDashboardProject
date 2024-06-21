namespace NupatDashboardProject.Models
{
	public class Assignment
	{
		public Guid AssignmentId { get; set; }
		public string AssignmentTitle { get; set; }
		public string Remarks { get; set; }
		public int Marks { get; set; }
		public DateTime SubmissionDate { get; set; } 
	}
}
