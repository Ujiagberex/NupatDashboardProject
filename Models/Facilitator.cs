using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
	public class Facilitator
    {
        [Key]
        public Guid FacilitatorId { get; set; }
        public string FullName { get; set; }
        public string? Course { get; set; }


    }
}
