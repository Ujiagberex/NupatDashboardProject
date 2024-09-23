using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
    public interface IProfile
    {
        void AddProfile(AddStudentProfileDTO addProfileDTO);
        Profile GetProfileById(Guid id);
		IEnumerable<Profile> GetAllProfiles();
		bool DeleteProfileById(Guid id);
        ShowProfileDTO UpdateProfileById(Profile profile);
    }
}
