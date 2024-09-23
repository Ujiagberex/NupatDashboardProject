using Microsoft.AspNetCore.Mvc;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class FacilitatorService : IFacilitator
	{
		private readonly LmsDbContext _dataContext;
		

		public FacilitatorService(LmsDbContext dataContext)
		{
			_dataContext = dataContext;
		}
		

		public bool DeleteFacilitatorById(string id)
		{
			Facilitator facilitator = _dataContext.Facilitators.Find(id);
			if (facilitator == null)
			{
				return false;
			}
			_dataContext.Remove(facilitator);
			_dataContext.SaveChanges();

			return true;
		}

		public IEnumerable<Facilitator> GetAllFacilitators()
		{
			return _dataContext.Facilitators.AsEnumerable();
		}

		public Facilitator GetfacilitatorById(string id)
		{
			return _dataContext.Facilitators.Find(id);
		}


		public ShowFaciltatorProfileDTO UpdateFacilitatorById(Facilitator facilitator)
		{
			var FindFacilitator = GetfacilitatorById(facilitator.Id);
			if (FindFacilitator == null)
			{
				return null;

			}
			FindFacilitator.FullName = facilitator.FullName;
			_dataContext.SaveChanges();
			ShowFaciltatorProfileDTO showFacilitatorDTO = new ShowFaciltatorProfileDTO();
			showFacilitatorDTO.FullName = facilitator.FullName;
			return showFacilitatorDTO;
		}
	

		
	}
}
