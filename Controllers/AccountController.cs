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
	public class AccountController(IAuth userAuth, 
		UserManager<ApplicationUser> userManager) : ControllerBase
	{
		[HttpPost("ResgisterStudent")]

		public async Task<IActionResult> RegisterNewStudent(RegisterStudentDTO StudentDTO)
		{
				var Response = await userAuth.RegisterStudent(StudentDTO);
				return Ok(Response);
		}

		[HttpPost("RegisterFacilitator")]

		public async Task<IActionResult> RegisterNewFacilitator(RegisterFacilitatorDTO FacilitatorDTO)
		{
			
				var Response = await userAuth.RegisterFacilitator(FacilitatorDTO);
				return Ok(Response);
			
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDTO loginUserDTO)
		{
			if (loginUserDTO == null || string.IsNullOrEmpty(loginUserDTO.UserName))
			{
				return BadRequest("Invalid login request");
			}

			var result = await userAuth.LoginUser(loginUserDTO);
			if (result.Item1)
			{
				return Ok(new { jwtToken = result.Item2 });
			}

			return Unauthorized("Invalid login attempt");
		}

		[HttpPost("ChangePassword")]
		public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
		{
			var result = await userAuth.ChangePasswordAsync(changePasswordDTO);
			if (!result.Succeeded)
			{
				return BadRequest(result);
			}

			return Ok(result);
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
