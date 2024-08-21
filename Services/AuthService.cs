using System.IO.Compression;
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
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IConfiguration _configuration;
		private readonly ILogger<AuthService> _logger;


		public AuthService(RoleManager<IdentityRole> roleManager, 
			UserManager<ApplicationUser> userManager, 
			ITokenGenerator tokenGenerator, 
			IConfiguration configuration,
			ILogger<AuthService> logger)
		{
			_logger = logger;
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


		//public async Task<(bool, AuthResponse)> LoginUser(LoginDTO loginUserDTO, ApplicationUser user)
		//{
		//	var result = await _signInManager.PasswordSignInAsync(user.UserName, loginUserDTO.Password, false, lockoutOnFailure: false);
		//	if (result.Succeeded)
		//	{
		//		var jwtToken = await _tokenGenerator.GenerateJwtToken(user.Id, user.PhoneNumber, user.UserName, user.Email, user.FullName);
		//		return (true, jwtToken);
		//	}

		//	return (false, null);
		//}


		public async Task<(bool, AuthResponse)> LoginUser(LoginDTO loginUserDTO)
		{
			if (loginUserDTO == null)
			{
				throw new ArgumentNullException(nameof(loginUserDTO));
			}


			// Check user has a registered email
			var user = await _userManager.FindByEmailAsync(loginUserDTO.UserName);
			if (user == null || loginUserDTO.Password != "Nupat_24")
			{
				return (false, null);

			}
			var roles = await _userManager.GetRolesAsync(user);
			var jwtToken = await _tokenGenerator.GenerateJwtToken(user.Id, user.PhoneNumber, user.UserName, user.Email, user.FullName, roles);
			
			return  (true, jwtToken);
		}

		public async Task<AuthResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
		{
			if (changePasswordDTO == null)
			{
				throw new ArgumentNullException(nameof(changePasswordDTO));
			}

			var user = await _userManager.FindByEmailAsync(changePasswordDTO.UserName);
			if (user == null)
			{
				_logger.LogWarning($"User with username {changePasswordDTO.UserName} not found.");
				return new AuthResult { Succeeded = false, Message = "User not found." };
			}


			// Attempt to change the password
			var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.Password, changePasswordDTO.NewPassword);
			if (!result.Succeeded)
			{
				_logger.LogWarning($"Password change attempt failed for user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
				return new AuthResult { Succeeded = false, Message = "Password change failed.", Errors = result.Errors.Select(e => e.Description).ToArray() };
			}

			_logger.LogInformation($"Password change successful for user {user.UserName}.");
			return new AuthResult { Succeeded = true, Message = "Password changed successfully." };
		}

		public async Task<string> RegisterFacilitator(RegisterFacilitatorDTO registerFacilitatorDTO)
		{

			ApplicationUser facilitator = new ApplicationUser()
			{
				Email = registerFacilitatorDTO.Email,
				FullName = registerFacilitatorDTO.FullName,
				Course = registerFacilitatorDTO.Course,
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
			//string defaultPassword = _configuration["DefaultPassword:"];

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
