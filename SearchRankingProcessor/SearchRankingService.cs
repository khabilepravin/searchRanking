using Models;
using SearchRankingHtmlProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchRankingProcessor
{
    public class SearchRankingService : ISearchRankingService
    {
        private readonly ISearchService _searchService;
        private readonly IHtmlProcessor _htmlProcessor;

        public SearchRankingService(ISearchService searchService, IHtmlProcessor htmlProcessor)
        {
            _searchService = searchService;
            _htmlProcessor = htmlProcessor;
        }

        public async Task<SearchRankingResult> GetSearchRanking(string host, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(host)) { throw new ArgumentException("Invalid host"); }
            if (string.IsNullOrWhiteSpace(searchTerm)) { throw new ArgumentException("Invalid searchTerm"); }

            var searchResult = await _searchService.Search(searchTerm);

            var searchResultUris = _htmlProcessor.FetchUrlsFromSearchResult(searchResult);

            var rankings = BuildRankings(searchResultUris);

            return (from rank in rankings
                    where rank.Host.IndexOf(host, StringComparison.InvariantCultureIgnoreCase) >= 0
                    select rank).FirstOrDefault<SearchRankingResult>();
        }

        public async Task<IEnumerable<SearchRankingResult>> GetAllSearchRankingResults(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) { throw new ArgumentException("Invalid searchTerm"); }
            var searchResult = await _searchService.Search(searchTerm);

            var searchResultUris = _htmlProcessor.FetchUrlsFromSearchResult(searchResult);

            return BuildRankings(searchResultUris);
        }

        private IEnumerable<SearchRankingResult> BuildRankings(List<Uri> searchResultUrls)
        {
            return from url in searchResultUrls
                   select new SearchRankingResult
                   {
                       Host = url.Host,
                       Ranking = searchResultUrls.IndexOf(url)
                   };
        }
    }
}
