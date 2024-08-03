using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.DTO
{
	public class RegisterFacilitatorDTO
	{
		[Required]
		[EmailAddress]
		[StringLength(70)]
		public string Email { get; set; }
		[Required]
		[StringLength(100)]
		public string FullName {  get; set; }
		[Required]
		[StringLength(100)]
		public string Course { get; set; }
      
    }
}
