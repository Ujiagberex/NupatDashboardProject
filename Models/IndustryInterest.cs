namespace NupatDashboardProject.Models
{
    public class IndustryInterest
    {
		public Guid Id { get; set; }
		public string Interest { get; set; }
		public Guid ProfileId { get; set; }
		public Profile Profile { get; set; }
	}
}
