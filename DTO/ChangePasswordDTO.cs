using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.DTO
{
	public class ChangePasswordDTO : LoginDTO
	{
       
        
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		//[Required]
		//[DataType(DataType.Password)]
		//[Compare(nameof(NewPassword))]
		//public string ConfirmNewPassword { get; set; }
	}
}
