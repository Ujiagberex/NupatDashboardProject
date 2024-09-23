using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class PopularCourseRepo : ICourseRepository
	{
		private readonly LmsDbContext _context;

        public PopularCourseRepo(LmsDbContext context)
        {
            _context = context;
        }
		
	}
}
