using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;
using NupatDashboardProject.RoleNameEnum;

namespace NupatDashboardProject.Services
{
	public class AuthService : IAuth
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IConfiguration _configuration;

		public AuthService(RoleManager<IdentityRole> roleManager, 
			UserManager<ApplicationUser> userManager, 
			SignInManager<ApplicationUser> signInManager,
			ITokenGenerator tokenGenerator, 
			IConfiguration configuration)
		{
			_signInManager = signInManager;
			_roleManager = roleManager;
			_userManager = userManager;
			_tokenGenerator = tokenGenerator;
			_configuration = configuration;
		}

		public async Task<ApplicationUser> FindUserByUsername(string userName)
		{
			var user = await _userManager.FindByEmailAsync(userName);
			return user;
		}
			

		public async Task<(bool, AuthResponse)> LoginUser(LoginDTO loginUserDTO, ApplicationUser user)
		{
			var result = await _signInManager.PasswordSignInAsync(user.UserName, loginUserDTO.Password, false, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				var jwtToken = await _tokenGenerator.GenerateJwtToken(user.Id, user.PhoneNumber, user.UserName, user.Email, user.FullName);
				return (true, jwtToken);
			}

			return (false, null);
		}


		//public async Task<(bool, AuthResponse)> LoginUser(LoginDTO loginUserDTO, ApplicationUser user)
		//{
		//	// check user with password valid
		//	var IsPassword = await _userManager.CheckPasswordAsync(user, loginUserDTO.Password);
		//	if (IsPassword)
		//	{
		//		// generate token
		//		var jwtToken = await _tokenGenerator.GenerateJwtToken(user.Id, user.PhoneNumber, user.UserName, user.Email,user.FullName);
		//		return (true, jwtToken);
		//	}
		//	return (false, null);
		//}

		public async Task<string> RegisterFacilitator(RegisterFacilitatorDTO registerFacilitatorDTO)
		{
			string defaultPassword = _configuration["DefaultPassword:"];

			ApplicationUser facilitator = new ApplicationUser()
			{
				Email = registerFacilitatorDTO.Email,
				FullName = registerFacilitatorDTO.FullName,
				Course = registerFacilitatorDTO.Course,
				Cohort = null,
				EmailConfirmed = true,
				UserName = registerFacilitatorDTO.Email
			};

			//Create User
			var createResult = await _userManager.CreateAsync(facilitator);
			if (createResult.Succeeded)
			{

				var roleExist = await _roleManager.RoleExistsAsync(RoleName.Facilitator.ToString());
				if (!roleExist)
				{
					// add role to application
					IdentityRole role = new IdentityRole(RoleName.Facilitator.ToString());
					var RoleResult = await _roleManager.CreateAsync(role);
					if (!RoleResult.Succeeded)
					{
						return RoleResult.Errors.First().Description;
					}
				}
				// add user to role
				var AddFacilitatorToRole = await _userManager.AddToRoleAsync(facilitator, RoleName.Facilitator.ToString());
				if (AddFacilitatorToRole.Succeeded)
				{
					return "Successful";
				}
				else
				{
					return AddFacilitatorToRole.Errors.First().Description;
				}

			}
			return createResult.Errors.First().Description;

		}

		public async Task<string> RegisterStudent(RegisterStudentDTO registerStudentDTO)
		{
			string defaultPassword = _configuration["DefaultPassword:"];

			ApplicationUser student = new ApplicationUser()
			{
				Email = registerStudentDTO.Email,
				FullName = registerStudentDTO.FullName,
				Course = registerStudentDTO.Course,
				Cohort = registerStudentDTO.Cohorts,
				EmailConfirmed = true,
				UserName = registerStudentDTO.Email
			};

			// create user
			var createResult = await _userManager.CreateAsync(student);
			if(createResult.Succeeded) 
			{ 

			var roleExist = await _roleManager.RoleExistsAsync(RoleName.Student.ToString());
			if (!roleExist)
			{
				// add role to application
				IdentityRole role = new IdentityRole(RoleName.Student.ToString());
				var RoleResult = await _roleManager.CreateAsync(role);
				if (!RoleResult.Succeeded)
				{
					return RoleResult.Errors.First().Description;
				}
			}
			// add user to role
			var AddStudentToRole = await _userManager.AddToRoleAsync(student, RoleName.Student.ToString());
			if (AddStudentToRole.Succeeded)
			{
				return "Successful";
			}
			else
			{
				return AddStudentToRole.Errors.First().Description;
			}

			}
			return createResult.Errors.First().Description;
		}
	}
}
