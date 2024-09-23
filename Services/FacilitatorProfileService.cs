using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
    public class FacilitatorProfileService : IFacilitatorProfileService
    {
        private readonly LmsDbContext _dbContext;

        public FacilitatorProfileService(LmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddFacilatorProfile(AddFaciltatorProfileDTO addProfileDTO)
        {
            FacilitatorProfile profile = new FacilitatorProfile
            {
                Id = Guid.NewGuid(),
                FullName = addProfileDTO.FullName,
                EmailAddress = addProfileDTO.EmailAddress,
                PhoneNumber = addProfileDTO.PhoneNumber,
                State = addProfileDTO.State,

            };
            _dbContext.facilitatorProfiles.Add(profile);
            _dbContext.SaveChanges();
        }

        public bool DeleteProfileById(Guid id)
        {
            FacilitatorProfile response = _dbContext.facilitatorProfiles.Find(id);
            if (response == null)
            {
                return false;
            }

            _dbContext.facilitatorProfiles.Remove(response);
            _dbContext.SaveChanges();

            return true;
        }

        public IEnumerable<FacilitatorProfile> GetAllProfiles()
        {
            return _dbContext.facilitatorProfiles.AsEnumerable();
        }

        public FacilitatorProfile GetFaProfileById(Guid id)
        {
            return _dbContext.facilitatorProfiles.Find(id);
        }

        public ShowFaciltatorProfileDTO UpdateProfileById(FacilitatorProfile profile)
        {
            var findProfile = GetFaProfileById(profile.Id);
            if (findProfile == null)
            {
                return null;
            }

            findProfile.FullName = profile.FullName;
            findProfile.PhoneNumber = profile.PhoneNumber;
            findProfile.EmailAddress = profile.EmailAddress;
            findProfile.State = profile.State;

            _dbContext.SaveChanges();

            ShowFaciltatorProfileDTO showProfileDto = new ShowFaciltatorProfileDTO();
            showProfileDto.FullName = profile.FullName;

            return showProfileDto;
        }
    }
}
