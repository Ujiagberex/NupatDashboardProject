namespace NupatDashboardProject.Models
{
    public class SocialMediaAccount
    {
		public Guid Id { get; set; }
		public string Platform { get; set; }
		public string Username { get; set; }
		public Guid ProfileId { get; set; }
		public Profile Profile { get; set; }

	}
}
