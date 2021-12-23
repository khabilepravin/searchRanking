using Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchRankingProcessor
{
    public class SearchService : ISearchService
    {
        private readonly SearchSettings _searchSettings;
        public SearchService(SearchSettings searchSettings)
        {
            _searchSettings = searchSettings;
        }

        public async Task<string> Search(string searchTerm)
        {
            if(_searchSettings.SearchLimit <= 0) { throw new ArgumentException("Invalid SearchLimit"); }
            if(string.IsNullOrWhiteSpace(_searchSettings.SearchUrl) || Uri.IsWellFormedUriString(_searchSettings.SearchUrl, UriKind.Absolute) == false) { throw new ArgumentException("Invalid SearchUrl"); }
            if (string.IsNullOrWhiteSpace(searchTerm)) { throw new ArgumentException("Invalid searchTerm"); }

            var requestUrl = $"{_searchSettings.SearchUrl}search?num={_searchSettings.SearchLimit}&q={searchTerm}";
            var httpClient = new HttpClient();
            var request = await httpClient.GetAsync(requestUrl);
         
            return await request.Content.ReadAsStringAsync();
        }
    }
}
