namespace NupatDashboardProject.Models
{
	public class ClassSchedule
	{
		public int Id { get; set; }
		public string Topic { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan Duration { get; set; }
		public string Cohort { get; set; }
	}

	public class Event
	{
		public int Id { get; set; }
		public string ImagePath { get; set; }
		public string EventLink { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan Time { get; set; }
	}
}
