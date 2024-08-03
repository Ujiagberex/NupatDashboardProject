using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;
using NupatDashboardProject.Services;

namespace NupatDashboardProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(IAuth userAccount, 
		UserManager<ApplicationUser> userManager) : ControllerBase
	{
		[HttpPost("ResgisterStudent")]

		public async Task<IActionResult> RegisterNewStudent(RegisterStudentDTO newStudentDTO)
		{
				var Response = await userAccount.RegisterStudent(newStudentDTO);
				return Ok(Response);
		}

		[HttpPost("RegisterFacilitator")]

		public async Task<IActionResult> RegisterNewFacilitator(RegisterFacilitatorDTO newFacilitatorDTO)
		{
			
				var Response = await userAccount.RegisterFacilitator(newFacilitatorDTO);
				return Ok(Response);
			
		}

		[HttpPost("Login")]
		public async Task<IActionResult> LoginNewUser(LoginDTO login)
		{
			var user = await userAccount.FindUserByUsername("email");
			if (user == null)
			{
				return NotFound();
			}
			var result = await userAccount.LoginUser(login, user);
			if (result.Item1 == true)
			{
				return Ok(result.Item2);
			}

			return BadRequest();
		}

		//[HttpPost("LogIn")]

		//public async Task<IActionResult> UserLogIn([FromBody] LoginDTO loginDTO)
		//{
		//	// Find the user by email
		//	var user = await userManager.FindByEmailAsync(loginDTO.Username);
		//	if (user == null)
		//	{
		//		return Unauthorized(new { flag = false, token = (string)null, message = "User not found" });
		//	}

		//	// Try to login the user
		//	var (isAuthenticated, authResponse) = await userAccount.LoginUser(loginDTO, user);
		//	if (!isAuthenticated)
		//	{
		//		return Unauthorized(new { flag = false, token = (string) null, message = "Invalid credentials" });
		//	}

		//	// Return the token
		//	return Ok(new { flag = true, token = authResponse.AccessToken, expiration = authResponse.ExpiryTime, message = "Login successful" });
		//}

	}
}
