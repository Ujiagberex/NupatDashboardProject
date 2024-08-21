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
		[HttpPost("RegisterStudent")]

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
			return Ok();
		}
	}
}
