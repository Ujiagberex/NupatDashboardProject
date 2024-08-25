using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.DTO;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface IFacilitator
	{
		void AddFacilitator(AddFacilitatorDTO addFacilitatorDTO);
		Facilitator GetfacilitatorById(Guid id);
		IEnumerable<Facilitator> GetAllFacilitators();
		bool DeleteFacilitatorById(Guid id);
		ShowFacilitatorDTO UpdateFacilitatorById(Facilitator facilitator);
	}
}
