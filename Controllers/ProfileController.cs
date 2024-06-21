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

		//Upload Photo
		[HttpPost("uploadPhoto")]
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
		[HttpDelete("delete/{publicId}")]
		private async Task<IActionResult> DeletePhoto(string publicId)
		{
			var result = await _photoService.DeletePhotoAsync(publicId);

			if (result.Result == "ok")
			{
				return Ok();
			}

			return BadRequest(result.Error.Message);
		}

		//Get photo by Id
		[HttpGet("GetPhotoBy{publicId}")]
		public string GetPublicIdFromCloudinaryUrl(string url)
		{
			var uri = new Uri(url);
			var segments = uri.Segments;
			var publicId = segments.LastOrDefault()?.Split('.').FirstOrDefault();
			return publicId;
		}

		//Get all photos with a limit of 500
		[HttpGet("GetallPhotos(500)")]
		public async Task<IActionResult> GetAllPhotos()
		{
			var result = await _photoService.GetAllPhotosAsync();
			return Ok(result);
		}

		//Get Profile By Id
		[HttpGet("GetProfileBy{id}")]
		public IActionResult GetProfileById(Guid id)
		{
			var FindProfile = _profile.GetProfileById(id);
			if (FindProfile == null)
			{
				return BadRequest("student does not exist");
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
