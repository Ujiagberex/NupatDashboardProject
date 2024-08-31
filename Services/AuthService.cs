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


		//Login Service
		//public async Task<(bool, AuthResponse)> LoginUser(LoginDTO loginUserDTO)
		//{
		//	if (loginUserDTO == null)
		//	{
		//		throw new ArgumentNullException(nameof(loginUserDTO));
		//	}


		//	// Check user has a registered email
		//	var user = await _userManager.FindByEmailAsync(loginUserDTO.UserName);
		//	if (user == null )
		//	{
		//		return (false, null);

		//	}
		//	// Check if the provided password is the fixed password
		//	if (loginUserDTO.Password != "Nupat_24")
		//	{
		//		// Check if the provided password is correct
		//		var passwordValid = await _userManager.CheckPasswordAsync(user, loginUserDTO.Password);
		//		if (!passwordValid)
		//		{
		//			return (false, null);
		//		}
		//	}

		//	var roles = await _userManager.GetRolesAsync(user);
		//	var jwtToken = await _tokenGenerator.GenerateJwtToken(user.Id, user.PhoneNumber, user.UserName, user.Email, user.FullName, roles);

		//	return  (true, jwtToken);
		//}

		public async Task<(bool, AuthResponse)> LoginUser(LoginDTO loginUserDTO)
		{
			if (loginUserDTO == null)
			{
				throw new ArgumentNullException(nameof(loginUserDTO));
			}

			// Check if the user has a registered email
			var user = await _userManager.FindByEmailAsync(loginUserDTO.UserName);
			if (user == null)
			{
				return (false, null);
			}

			// Check if the fixed password can be used
			if (!user.IsPasswordChanged && loginUserDTO.Password == "Nupat_24")
			{
				// Proceed to generate the token
				var userRoles = await _userManager.GetRolesAsync(user);
				var _jwtToken = await _tokenGenerator.GenerateJwtToken(user.Id, user.PhoneNumber, user.UserName, user.Email, user.FullName, userRoles);
				return (true, _jwtToken);
			}

			// Validate the actual password
			var passwordValid = await _userManager.CheckPasswordAsync(user, loginUserDTO.Password);
			if (!passwordValid)
			{
				return (false, null);
			}

			// Proceed to generate the token
			var roles = await _userManager.GetRolesAsync(user);
			var jwtToken = await _tokenGenerator.GenerateJwtToken(user.Id, user.PhoneNumber, user.UserName, user.Email, user.FullName, roles);
			return (true, jwtToken);
		}



		//Sign-up facilitator
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
			// Set the fixed password
			var password = "Nupat_24";

			var createResult = await _userManager.CreateAsync(facilitator, password);
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
			// Set the fixed password
			var password = "Nupat_24";

			var createResult = await _userManager.CreateAsync(student, password);
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

		//Change password service
		public async Task<(bool, string)> ChangePassword(ChangePasswordDTO changePasswordDTO)
		{
			if (changePasswordDTO == null)
			{
				throw new ArgumentNullException(nameof(changePasswordDTO));
			}

			// Find the user by username
			var user = await _userManager.FindByNameAsync(changePasswordDTO.UserName);
			if (user == null)
			{
				return (false, "User not found.");
			}

			// Check if the current password is the fixed password and the user has not changed their password before
			if (changePasswordDTO.CurrentPassword == "Nupat_24" && !user.IsPasswordChanged)
			{
				// Remove old password and add the new one
				var removeResult = await _userManager.RemovePasswordAsync(user);
				if (!removeResult.Succeeded)
				{
					return (false, "Failed to remove old password.");
				}

				var addResult = await _userManager.AddPasswordAsync(user, changePasswordDTO.NewPassword);
				if (addResult.Succeeded)
				{
					// Mark the user as having changed their password
					user.IsPasswordChanged = true;
					await _userManager.UpdateAsync(user);

					return (true, "Password changed successfully.");
				}
				else
				{
					return (false, string.Join(", ", addResult.Errors.Select(e => e.Description)));
				}
			}

			// Validate the current password against the stored password
			var passwordValid = await _userManager.CheckPasswordAsync(user, changePasswordDTO.CurrentPassword);
			if (!passwordValid)
			{
				return (false, "Incorrect password.");
			}

			// Change the password using the provided new password
			var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
			if (changePasswordResult.Succeeded)
			{
				// Mark the user as having changed their password
				user.IsPasswordChanged = true;
				await _userManager.UpdateAsync(user);

				return (true, "Password changed successfully.");
			}

			return (false, string.Join(", ", changePasswordResult.Errors.Select(e => e.Description)));
		}


	}
}
