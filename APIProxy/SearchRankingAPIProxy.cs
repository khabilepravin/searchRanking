using APIProxy.ResposeModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Util;
using System.Configuration;

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
