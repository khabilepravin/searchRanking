using APIProxy.ResposeModels;
using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace APIProxy
{
    public class SearchRankingAPIProxy : ISearchRankingAPIProxy
    {
        private string _apiBaseUrl;

        public SearchRankingAPIProxy()
        {
           _apiBaseUrl = ConfigurationManager.AppSettings["APIBaseUrl"];
        }

        public async Task<IEnumerable<SearchRankingResult>> GetAllRankingResults(string searchTerm)
        {
            var result = await $"{_apiBaseUrl}/searchranking/all"
                .SetQueryParam("searchTerm", searchTerm)
                .GetJsonAsync<IEnumerable<SearchRankingResult>>();

            return result;
        }

        public async Task<SearchRankingResult> GetRankingResultByHost(string host, string searchTerm)
        {            
            var result = await $"{_apiBaseUrl}/searchranking"
                           .SetQueryParams(new { host = host, searchTerm = searchTerm })
                           .GetJsonAsync<SearchRankingResult>();
            
            return result;
        }
    }
}
