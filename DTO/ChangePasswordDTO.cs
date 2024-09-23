using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.DTO
{
	public class ChangePasswordDTO
	{
		public string UserName { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
