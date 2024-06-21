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
		public async Task<IEnumerable<Course>> GetMostPopularCoursesAsync(int limit)
		{
			return await _context.Courses
				.Include(c => c.Students)
				.OrderByDescending(c => c.Students.Count)
				.Take(limit)
				.ToListAsync();
		}
	}
}
