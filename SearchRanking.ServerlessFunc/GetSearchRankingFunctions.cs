using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Models;
using SearchRankingProcessor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchRanking.ServerlessFunc
{
    public class GetSearchRankingFunctions
    {
        private readonly ISearchRankingService _searchRankingService;
        public GetSearchRankingFunctions(ISearchRankingService searchRankingService)
        {
            _searchRankingService = searchRankingService;
        }

        [FunctionName("GetSearchRankingByDomain")]
        public async Task<IActionResult> GetSearchRankingByDomain(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            SearchRankingResult result = null;
            try
            {
                log.LogInformation("GetSearchRanking request started");

                string domain = req.Query["domain"];
                string searchTerm = req.Query["searchTerm"];

                result = await _searchRankingService.GetSearchRanking(domain, searchTerm);

                log.LogInformation("GetSearchRanking request processed successfully");
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing the request {ex.Message}");
            }
            finally
            {
                log.LogInformation("GetSearchRanking request finished");
            }
           
            return new OkObjectResult(result);
        }

        [FunctionName("GetAllSearchRankings")]
        public async Task<IActionResult> GetAllSearchRankings(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            IEnumerable<SearchRankingResult> result = null;
            try
            {
                log.LogInformation("GetAllSearchRanking request started");

                string searchTerm = req.Query["searchTerm"];

                result = await _searchRankingService.GetAllSearchRankingResults(searchTerm);

                log.LogInformation("GetAllSearchRanking request processed successfully");
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing the request {ex.Message}");
            }
            finally
            {
                log.LogInformation("GetAllSearchRanking request finished");
            }

            return new OkObjectResult(result);
        }
    }
}
