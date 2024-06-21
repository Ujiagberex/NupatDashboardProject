namespace NupatDashboardProject.DTO
{
	public class UploadContentDTO
	{
		public Guid CohortId { get; set; }
		public IFormFile File { get; set; }
	}
}
