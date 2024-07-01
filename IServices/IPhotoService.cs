using CloudinaryDotNet.Actions;

namespace NupatDashboardProject.IServices
{
    public interface IPhotoService
    {
        Task<ImageUploadResult>AddPhotoAsync(IFormFile file); 
		Task<ListResourcesResult> GetAllPhotosAsync();
		Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<ImageUploadResult> AddCoverPhotoPhotoAsync(IFormFile file);
		Task<DeletionResult> DeleteCoverPhotoAsync(string publicId);
		Task<ListResourcesResult> GetAllCoverPhotosAsync();
	}
}
