using NupatDashboardProject.Models;

namespace NupatDashboardProject.IServices
{
	public interface ISearchService
	{
		Task<SearchResponse> SearchAsync(string query, int page, int limit);
	}
}
