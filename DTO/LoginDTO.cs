using System.ComponentModel.DataAnnotations;


namespace NupatDashboardProject.DTO
{
	//public record LoginDTO([EmailAddress] string Username, string Password="NupatStudent_24");
	public class LoginDTO
	{
		[Required]
		[EmailAddress]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }

	}
}
