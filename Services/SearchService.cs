using Microsoft.Extensions.Options;
using Nest;
using NupatDashboardProject.Configurations;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Services
{
	public class SearchService : ISearchService
	{
		private readonly IElasticClient _elasticClient;
		private readonly ElasticSearchConfig _config;

		public SearchService(IElasticClient elasticClient, IOptions<ElasticSearchConfig> config)
		{
			_elasticClient = elasticClient;
			_config = config.Value;
		}

		public async Task<SearchResponse> SearchAsync(string query, int page, int limit)
		{
			var response = await _elasticClient.SearchAsync<dynamic>(s => s
				.Index(_config.Index)
				.From((page - 1) * limit)
				.Size(limit)
				.Query(q => q
					.MultiMatch(m => m
						.Query(query)
						.Fields(f => f
							.Field("title")
							.Field("content")
						)
					)
				)
			);

			if (!response.IsValid)
			{
				throw new Exception(response.DebugInformation);
			}

			return new SearchResponse
			{
				Query = query,
				Page = page,
				Limit = limit,
				TotalResults = (int)response.Total,
				Results = response.Hits.Select(hit => new SearchResult
				{
					Title = hit.Source.title,
					Snippet = hit.Source.content.Substring(0, Math.Min(200, hit.Source.content.Length)),
					Url = hit.Source.url
				}).ToList()
			};
		}


	}
}
