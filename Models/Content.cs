namespace NupatDashboardProject.Models
{
	public class Content
	{
		public Guid ContentId { get; set; }
		public string Owner { get; set; }
		public string FileName { get; set; }
		public byte[]? FileData { get; set; }
		public DateTime UploadDate { get; set; }
	}
}
