using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
	public class Facilitator
    {
        [Key]
        public Guid FacilitatorId { get; set; }
        public string FullName { get; set; }
        public string? Course { get; set; }

    }

	public class StudentResource
    {
		public int Id { get; set; }
		public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
		public DateTime DateUploaded { get; set; }
	}
}
