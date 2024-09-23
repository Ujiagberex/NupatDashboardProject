using System.ComponentModel.DataAnnotations;

namespace NupatDashboardProject.Models
{
    public class FacilitatorProfile
    {
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        
    }
}
