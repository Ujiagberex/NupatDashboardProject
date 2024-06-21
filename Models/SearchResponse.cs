using CloudinaryDotNet.Actions;

namespace NupatDashboardProject.Models
{
	public class SearchResponse
	{
		public string Query { get; set; }
		public int Page { get; set; }
		public int Limit { get; set; }
		public int TotalResults { get; set; }
		public List<SearchResult> Results { get; set; }
	}
	public class SearchResult
	{
		public string Title { get; set; }
		public string Snippet { get; set; }
		public string Url { get; set; }
	}
}
