namespace NupatDashboardProject.DTO
{
	public class SchduleClassDTO
	{
		public string? Facilitator { get; set; }
		public DateTime Date { get; set; }
		public int Time { get; set; }
		public string Duration { get; set; }
		public List<string> Participants { get; set; } = new List<string>();

	}
} 
