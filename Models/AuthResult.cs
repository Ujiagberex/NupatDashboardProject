namespace NupatDashboardProject.Models
{
	public class AuthResult
	{
		public bool Succeeded { get; set; }
		public string Message { get; set; }
		public string[] Errors { get; set; }
	}
}
