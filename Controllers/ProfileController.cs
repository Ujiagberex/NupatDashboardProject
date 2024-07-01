using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;
using NupatDashboardProject.Services;

namespace NupatDashboardProject.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private readonly IProfile _profile;
		private readonly IPhotoService _photoService;

		public ProfileController( IProfile profile, IPhotoService photoService)
		{
			
			_profile = profile;
			_photoService = photoService;

		}

		[HttpPost("UploadCoverPhoto")]

		public async Task<IActionResult> UploadCoverPhoto(IFormFile file)
		{
			var result = await _photoService.AddCoverPhotoPhotoAsync(file);

			if (result.Error != null)
			{
				return BadRequest(result.Error.Message);
			}

			return Ok(new
			{
				Url = result.SecureUrl.AbsoluteUri,
				result.PublicId
			});

		}

		//Delete CoverPhoto by Id
		[HttpDelete("DeleteCoverPhotoby{publicId}")]
		public async Task<IActionResult> DeleteCoverPhoto(string publicId)
		{
			// Check if publicId is not null or empty
			if (string.IsNullOrEmpty(publicId))
			{
				return BadRequest("Invalid public ID.");
			}

			// Call the delete method from the service
			var result = await _photoService.DeleteCoverPhotoAsync(publicId);

			// Ensure result is not null
			if (result == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
			}

			// Check if the delete operation was successful
			if (result.Result == "ok")
			{
				return Ok();
			}

			// Handle cases where the Error property is null
			if (result.Error != null)
			{
				return BadRequest(result.Error.Message);
			}
			else
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
			}
		}

		//Get cover photo by ID
		[HttpGet("GetCoverPhotoBy{publicId}")]
		public string GetCoverPhotoPublicIdFromCloudinaryUrl(string url)
		{
			var uri = new Uri(url);
			var segments = uri.Segments;
			var publicId = segments.LastOrDefault()?.Split('.').FirstOrDefault();
			return publicId;
		}

		//Get all cover photos with a limit of 500
		[HttpGet("GetallCoverPhotos(500)")]
		public async Task<IActionResult> GetAllCoverPhotos()
		{
			var result = await _photoService.GetAllCoverPhotosAsync();
			return Ok(result);
		}

		//Upload Photo
		[HttpPost("UploadProfilePhoto")]
		public async Task<IActionResult> UploadPhoto(IFormFile file)
		{
			var result = await _photoService.AddPhotoAsync(file);

			if (result.Error != null)
			{
				return BadRequest(result.Error.Message);
			}

			return Ok(new
			{
				Url = result.SecureUrl.AbsoluteUri,
				result.PublicId
			});
		}

		//Delete Photo by Id
		[HttpDelete("DeleteProfilePhoto/{publicId}")]
		public async Task<IActionResult> DeletePhoto(string publicId)
		{
			var result = await _photoService.DeletePhotoAsync(publicId);

			if (result.Result == "ok")
			{
				return Ok();
			}

			return BadRequest(result.Error.Message);
		}

		//Get photo by Id
		[HttpGet("GetProfilePhotoBy{publicId}")]
		public string GetPublicIdFromCloudinaryUrl(string url)
		{
			var uri = new Uri(url);
			var segments = uri.Segments;
			var publicId = segments.LastOrDefault()?.Split('.').FirstOrDefault();
			return publicId;
		}

		//Get all photos with a limit of 500
		[HttpGet("GetallProfilePhotos(500)")]
		public async Task<IActionResult> GetAllPhotos()
		{
			var result = await _photoService.GetAllPhotosAsync();
			return Ok(result);
		}

		//Get Profile By Id
		[HttpGet("GetProfilePhotoBy{id}")]
		public IActionResult GetProfileById(Guid id)
		{
			var FindProfile = _profile.GetProfileById(id);
			if (FindProfile == null)
			{
				return BadRequest("Picture does not exist");
			}
			return Ok(FindProfile);
		}




		//Create Profile
		[HttpPost]
		[Route("CreateProfile")]
		public IActionResult AddProfile(AddProfileDTO profile)
		{
			_profile.AddProfile(profile);

			return Ok("Successful");
		}

		//Get all Profiles
		[HttpGet]
		[Route("GetAllProfile")]
		public IActionResult GetAllProfiles()
		{
			var profiles = _profile.GetAllProfiles();
			return Ok(profiles);
		}

		//Update Profile By Id
		[HttpPut("UpdateProfileBy{Id}")]
		public IActionResult UpdateStudentById(Profile profile)
		{
			var update = _profile.UpdateProfileById(profile);
			if (update == null)
			{
				return NotFound();
			}
			return Ok(update);

		}

		//Delete profile by Id
		[HttpDelete("DeleteProfileById/{id}")]
		public IActionResult DeleteProfileById(Guid id)
		{
			bool isDeleted = _profile.DeleteProfileById(id);
			if (!isDeleted)
			{
				return NotFound("Profile not found.");
			}
			return Ok("Profile deleted successfully.");
		}







	}
}
