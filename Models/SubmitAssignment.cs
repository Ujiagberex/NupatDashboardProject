using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NupatDashboardProject.Models
{
	public class SubmitAssignment
	{
        [Key]
        public Guid SubmitAssignmentId { get; set; }

        // Foreign key for Assignment
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        // Foreign key for Student
        [ForeignKey("Id")]
        public string Id { get; set; }
        public Student Student { get; set; } 

        public byte[] FileData { get; set; }
        public DateTime DateUploaded { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string FilePath { get; set; }
        public string? Status { get; set; }
    }
}
