using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;

namespace NupatDashboardProject.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

		//This is to restrict the content uploaded to the service
		private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png"};
		private readonly string[] _permittedMimeTypes = { "image/jpg", "image/jpeg", "image/png"};
        public PhotoService(Cloudinary cloudinary)
        {
           _cloudinary = cloudinary;
        }

		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
		{
			if (!IsImage(file))
			{
				throw new ArgumentException("Invalid file type. Only image files are allowed.");
			}

			var uploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				await using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Transformation = new Transformation().Crop("fill").Gravity("face").Width(500).Height(500)
				};
				uploadResult = await _cloudinary.UploadAsync(uploadParams);
			}
			return uploadResult;
		}

		public async Task<ListResourcesResult> GetAllPhotosAsync()
		{
			var listParams = new ListResourcesParams
			{
				MaxResults = 500
			};
			var result = await _cloudinary.ListResourcesAsync(listParams);
			return result;
		}

		public async Task<DeletionResult> DeletePhotoAsync(string publicId)
		{
			var deletionParams = new DeletionParams(publicId);
			var result = await _cloudinary.DestroyAsync(deletionParams);
			return result;
		}

		public async Task<ImageUploadResult> AddCoverPhotoPhotoAsync(IFormFile file)
		{
			if (!IsImage(file))
			{
				throw new ArgumentException("Invalid file type. Only image files are allowed.");
			}
			var coverUploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				await using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Transformation = new Transformation().Crop("fill").Width(851).Height(315)
				};
				coverUploadResult = await _cloudinary.UploadAsync(uploadParams);
			}
			return coverUploadResult;
		}

		public async Task<DeletionResult> DeleteCoverPhotoAsync(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);
			var result = await _cloudinary.DestroyAsync(deleteParams);

			return result;
		}

		public async Task<ListResourcesResult> GetAllCoverPhotosAsync()
		{
			var listParams = new ListResourcesParams
			{
				MaxResults = 500
			};
			var result = await _cloudinary.ListResourcesAsync(listParams);
			return result;
		}

		//Method to check the extention type and the Mime type
		private bool IsImage(IFormFile file)
		{
			var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
			if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
			{
				return false;
			}

			if (!_permittedMimeTypes.Contains(file.ContentType))
			{
				return false;
			}

			return true;
		}
	}

}
