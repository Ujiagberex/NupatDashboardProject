using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;

namespace NupatDashboardProject.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(Cloudinary cloudinary)
        {
           _cloudinary = cloudinary;
        }

		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
		{
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
			var deleteParams = new DeletionParams(publicId);
			var result = await _cloudinary.DestroyAsync(deleteParams);

			return result;
		}
	}

}
