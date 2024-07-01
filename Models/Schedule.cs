namespace NupatDashboardProject.Models
{
	public class Schedule
	{
		public Guid ScheduleId { get; set; }
		public string Title { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}
