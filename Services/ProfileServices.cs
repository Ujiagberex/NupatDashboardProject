using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class ProfileServices : IProfile
	{
		private readonly LmsDbContext _dbContext;

        public ProfileServices(LmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddProfile(AddProfileDTO addProfileDTO)
		{

			Profile profile = new Profile
			{
				ProfileId = Guid.NewGuid(),
				FullName = addProfileDTO.FullName,
				PhoneNumber = addProfileDTO.PhoneNumber,
				HomeAddress = addProfileDTO.HomeAddress,
				EmailAddress = addProfileDTO.EmailAddress,
				Bios = addProfileDTO.Bios,
				CourseOfInterest = addProfileDTO.CourseOfInterest,
				SocialMediaAccounts = addProfileDTO.SocialMediaAccounts,
				IndustryInterests = addProfileDTO.IndustryInterests
			};
			

			_dbContext.Profiles.Add(profile);
			_dbContext.SaveChanges();

		}


		public bool DeleteProfileById(Guid id)
		{
			Profile profile = _dbContext.Profiles.Find(id);
			if (profile == null)
			{
				return false;
			}
			_dbContext.Profiles.Remove(profile);
			_dbContext.SaveChanges();

			return true;
		}

		public IEnumerable<Profile> GetAllProfiles()
		{
			return _dbContext.Profiles.AsEnumerable();
		}

		public Profile GetProfileById(Guid id)
		{
			return _dbContext.Profiles.Find(id);
		}

		public ShowProfileDTO UpdateProfileById(Profile profile)
		{
			var FindProfile = GetProfileById(profile.ProfileId);
			if (FindProfile == null)
			{
				return null;

			}
			FindProfile.FullName = profile.FullName;
			FindProfile.PhoneNumber = profile.PhoneNumber;
			FindProfile.HomeAddress = profile.HomeAddress;
			FindProfile.EmailAddress = profile.EmailAddress;
			FindProfile.Bios = profile.Bios;
			FindProfile.CourseOfInterest = profile.CourseOfInterest;
			FindProfile.IndustryInterests = profile.IndustryInterests;
			FindProfile.SocialMediaAccounts = profile.SocialMediaAccounts;
			_dbContext.SaveChanges();
			ShowProfileDTO showProfileDTO = new ShowProfileDTO();
			showProfileDTO.FullName = profile.FullName;
			return showProfileDTO;

		}

		
	}
}
