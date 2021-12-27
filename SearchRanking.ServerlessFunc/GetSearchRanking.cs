using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SearchRankingProcessor;
using System.Threading.Tasks;

namespace SearchRanking.ServerlessFunc
{
    public class GetSearchRanking
    {
        private readonly ISearchRankingService _searchRankingService;
        public GetSearchRanking(ISearchRankingService searchRankingService)
        {
            _searchRankingService = searchRankingService;
        }

        [FunctionName("GetSearchRanking")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string domain = req.Query["domain"];
            string searchTerm = req.Query["searchTerm"];

            var result = await _searchRankingService.GetSearchRanking(domain, searchTerm);

            return new OkObjectResult(result);
        }
    }
}
