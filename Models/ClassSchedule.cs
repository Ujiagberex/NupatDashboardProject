namespace NupatDashboardProject.Models
{
	public class ClassSchedule
	{
		public int Id { get; set; }
		public string Topic { get; set; }
		public DateTime Date { get; set; }
		public string Duration { get; set; }
		public string Cohort { get; set; }
	}

	public class Event
	{
		public int Id { get; set; }
		public byte[] FileData { get; set; }
		public string EventLink { get; set; }
	}
}
