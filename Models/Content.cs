namespace NupatDashboardProject.Models
{
	public class Content
	{
		public Guid ContentId { get; set; }
		public string Owner { get; set; }
		public string FileName { get; set; }
		public byte[] FileData { get; set; }
		public DateTime UploadDate { get; set; }

        // Relationship: Content belongs to one Course
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
