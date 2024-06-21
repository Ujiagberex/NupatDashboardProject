using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;
using ServiceStack;

namespace NupatDashboardProject.Services
{
	public class FacilitatorService : IFacilitator
	{
		private readonly LmsDbContext _dataContext;

        public FacilitatorService(LmsDbContext dataContext)
        {
			_dataContext = dataContext;  
        }
        public void AddFacilitator(AddFacilitatorDTO addFacilitatorDTO)
		{

			Facilitator facilitator = new Facilitator
			{
				FacilitatorId = Guid.NewGuid(),
				FullName = addFacilitatorDTO.FullName,
				Course = addFacilitatorDTO.Course
			};

			 _dataContext.Facilitators.Add(facilitator);
			_dataContext.SaveChanges();
		}

		public bool DeleteFacilitatorById(Guid id)
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

		public Facilitator GetfacilitatorById(Guid id)
		{
			return _dataContext.Facilitators.Find(id);
		}

		public ShowFacilitatorDTO UpdateFacilitatorById(Facilitator facilitator)
		{
			var FindFacilitator = GetfacilitatorById(facilitator.FacilitatorId);
			if (FindFacilitator == null)
			{
				return null;

			}
			FindFacilitator.FullName = facilitator.FullName;
			_dataContext.SaveChanges();
			ShowFacilitatorDTO showFacilitatorDTO = new ShowFacilitatorDTO();
			showFacilitatorDTO.FullName = facilitator.FullName;
			return showFacilitatorDTO;
		}
	}
}
