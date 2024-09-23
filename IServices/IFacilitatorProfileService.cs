using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
    public interface IFacilitatorProfileService
    {
        void AddFacilatorProfile(AddFaciltatorProfileDTO addProfileDTO);
        FacilitatorProfile GetFaProfileById(Guid id);
        IEnumerable<FacilitatorProfile> GetAllProfiles();
        bool DeleteProfileById(Guid id);
        ShowFaciltatorProfileDTO UpdateProfileById(FacilitatorProfile profile);
    }
}
