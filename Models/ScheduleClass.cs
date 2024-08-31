namespace NupatDashboardProject.Models
{
	public class ScheduleClass
	{
        public Guid Id { get; set; }
        public string Facilitator { get; set; }
		public DateTime Date { get; set; }
		public int Time { get; set; }
		public string Duration { get; set; }
		public List<string> Participants { get; set; }
	}
}
