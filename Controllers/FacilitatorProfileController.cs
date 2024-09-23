using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitatorProfileController : ControllerBase
    {
        private readonly IFacilitatorProfileService _profile;

        public FacilitatorProfileController(IFacilitatorProfileService profile)
        {
            _profile = profile;
        }

        [HttpPost]
        [Route("CreateFacilitatorProfile")]
        public IActionResult AddProfile(AddFaciltatorProfileDTO profile)
        {
            _profile.AddFacilatorProfile(profile);

            return Ok("Successful");
        }

        [HttpGet("GetAllFacilitatorProfile")]
        public IActionResult GetAllProfile()
        {
            _profile.GetAllProfiles();
            return Ok("Successful");
        }

    }
}
