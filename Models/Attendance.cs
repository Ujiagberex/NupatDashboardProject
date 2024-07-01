namespace NupatDashboardProject.Models
{
	public class Attendance
	{
		public Guid AttendanceId { get; set; }
		public Guid StudentId { get; set; }
		public Guid CourseId { get; set; }
		public bool IsPresent { get; set; }
		public DateTime Date {  get; set; }
	}
}
