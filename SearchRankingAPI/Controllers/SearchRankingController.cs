using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Collections.Generic;

namespace SearchRankingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchRankingController : ControllerBase
    {
        
        public SearchRankingController()
        {

        }
        
        private readonly ILogger<SearchRankingController> _logger;

        public SearchRankingController(ILogger<SearchRankingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("search")]
        public IEnumerable<SearchRankingResult> Get([FromQuery]string searchTerm)
        {
            return null;
        }
    }
}
