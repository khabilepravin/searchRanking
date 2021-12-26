using APIProxy.ResposeModels;
using Flurl;
using Flurl.Http;
using Polly;
using System;
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
            return await Policy
                .Handle<FlurlHttpException>(IsWorthRetrying)
                .WaitAndRetryAsync(ExponentialBackoff())
                .ExecuteAsync(() => $"{_apiBaseUrl}/searchranking/all"
                    .SetQueryParam("searchTerm", searchTerm)
                    .GetJsonAsync<IEnumerable<SearchRankingResult>>());
        }

        public async Task<SearchRankingResult> GetRankingResultByHost(string host, string searchTerm)
        {
            return await Policy
                .Handle<FlurlHttpException>(IsWorthRetrying)
                .WaitAndRetryAsync(ExponentialBackoff())
                .ExecuteAsync(() => $"{_apiBaseUrl}/searchranking"
                    .SetQueryParams(new { host = host, searchTerm = searchTerm })
                    .GetJsonAsync<SearchRankingResult>());
        }

        private bool IsWorthRetrying(FlurlHttpException ex)
        {
            switch ((int)ex.Call.Response.StatusCode)
            {
                case 408:
                case 500:
                case 502:
                case 504:
                    return true;
                default:
                    return false;
            }
        }

        private IEnumerable<TimeSpan> ExponentialBackoff()
        {
            return new[]{
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(5)
                        };
        }
    }
}
