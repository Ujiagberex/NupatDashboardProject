using CloudinaryDotNet.Actions;

namespace NupatDashboardProject.IServices
{
    public interface IPhotoService
    {
        Task<ImageUploadResult>AddPhotoAsync(IFormFile file);
        Task<ListResourcesResult> GetAllPhotosAsync();
        Task<DeletionResult> DeletePhotoByIdAsync(string publicId);
		Task<ImageUploadResult> AddCoverPhotoPhotoAsync(IFormFile file);
		Task<DeletionResult> DeleteCoverPhotoAsync(string publicId);
        Task<List<string>> GetAllCoverPhotosAsync();

	}
}
