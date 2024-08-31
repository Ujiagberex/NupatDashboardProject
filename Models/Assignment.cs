using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
	public class Assignment
	{
		public Guid AssignmentId { get; set; }
		public byte[] FileData { get; set; }
		public DateTime DateUploaded { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime? SubmissionDate { get; set; }
		public string FilePath { get; set; }
		public string? Status { get; set; }

	}
}
