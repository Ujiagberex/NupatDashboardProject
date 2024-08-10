using Nest;
using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface IAuth
	{
		Task<string> RegisterStudent(RegisterStudentDTO registerStudentDTO);
		Task<string> RegisterFacilitator(RegisterFacilitatorDTO registerFacilitatorDTO);
		Task<AuthResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
		Task<(bool,AuthResponse)> LoginUser(LoginDTO loginUserDTO);
		Task<ApplicationUser> FindUserByUsername(string userName);
	}
}
