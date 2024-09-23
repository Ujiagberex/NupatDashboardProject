using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface IFacilitator
	{
		Facilitator GetfacilitatorById(string id);
		IEnumerable<Facilitator> GetAllFacilitators();
		bool DeleteFacilitatorById(string id);
        ShowFaciltatorProfileDTO UpdateFacilitatorById(Facilitator facilitator);
	}
}
