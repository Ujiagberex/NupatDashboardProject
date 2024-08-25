using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.DTO
{
	public class UploadContentDTO
	{

		[StringLength(50)]
        public string Owner { get; set; }
        public IFormFile File { get; set; }
	}
}
