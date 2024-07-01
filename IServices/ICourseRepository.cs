using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface ICourseRepository
	{
		Task<IEnumerable<Course>> GetMostPopularCoursesAsync(int limit);
	}
}
