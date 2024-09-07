namespace NupatDashboardProject.DTO
{
	public class SubmitAssignmentDTO
	{
        public Guid AssignmentId { get; set; }
		public string StudentId { get; set; }
		public IFormFile File { get; set; }
	}
}
