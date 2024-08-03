using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface IAuth
	{
		Task<string> RegisterStudent(RegisterStudentDTO registerStudentDTO);
		Task<string> RegisterFacilitator(RegisterFacilitatorDTO registerFacilitatorDTO);

		Task<(bool, AuthResponse)> LoginUser(LoginDTO loginUserDTO, ApplicationUser user);
		Task<ApplicationUser> FindUserByUsername(string userName);
	}
}
