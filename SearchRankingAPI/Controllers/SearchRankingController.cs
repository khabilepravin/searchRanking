using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using SearchRankingProcessor;
using System.Collections.Generic;

namespace SearchRankingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchRankingController : ControllerBase
    {
        private readonly IOptions<SearchSettings> _searchSettingOptions;
        private readonly ILogger<SearchRankingController> _logger;
        private readonly SearchSettings _searchSettings;
        private readonly ISearchRankingService _searchRankingService;

        public SearchRankingController(ILogger<SearchRankingController> logger, IOptions<SearchSettings> searchSettingOptions)
        {
            _logger = logger;
            _searchSettings = searchSettingOptions.Value;
        }

        [HttpGet]
        public ActionResult<SearchRankingResult> Get([FromQuery]string searchTerm)
        {
            return null;
        }

        [HttpGet("all")]
        public ActionResult<SearchRankingResult> GetAll([FromQuery] string searchTerm)
        {
            return null;
        }
    }
}
