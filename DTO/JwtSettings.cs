namespace NupatDashboardProject.DTO
{
	public class JwtSettings
	{
		public string Site { get; set; }
		public string Secret { get; set; }
		public TimeSpan TokenLifeTime { get; set; }
		public string Audience { get; set; }
	}
}
