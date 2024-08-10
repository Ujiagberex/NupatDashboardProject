using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.DTO
{
	public class ChangePasswordDTO
	{
        public string UserName { get; set; }
        public string OldPassword { get; set; }
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		//[Required]
		//[DataType(DataType.Password)]
		//[Compare(nameof(NewPassword))]
		//public string ConfirmNewPassword { get; set; }
	}
}
