using Microsoft.AspNetCore.Mvc;
using Models;
using SearchRankingProcessor;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using SearchRankingAPI.Diagnostics;

namespace SearchRankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchRankingController : ControllerBase
    {   
        private readonly ILogger<SearchRankingController> _logger;
        private readonly ISearchRankingService _searchRankingService;

        public SearchRankingController(ILogger<SearchRankingController> logger, ISearchRankingService searchRankingService)
        {
            _logger = logger;
            _searchRankingService = searchRankingService;
        }

        [HttpGet]     
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(SearchRankingResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<SearchRankingResult>> Get([FromQuery]string domain, [FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) { return BadRequest("Invalid searchTerm"); }
            if (string.IsNullOrWhiteSpace(domain)) { return BadRequest("Invalid domain"); }

            var result = await _searchRankingService.GetSearchRanking(domain, searchTerm);

            if (result == null) 
            { 
                _logger.LogWarning($"No search results found for {domain} with search term {searchTerm}");
                return NotFound($"No search results found for {domain} with searchTerm: {searchTerm}"); 
            }
                       
            return Ok(result);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SearchRankingResult>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<IEnumerable<SearchRankingResult>>> GetAll([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) { return BadRequest("Invalid searchTerm"); }

            var result = await _searchRankingService.GetAllSearchRankingResults(searchTerm);

            if (result?.Any() == false) 
            {
                _logger.LogWarning($"No search results found for searchTerm {searchTerm}");
                return NotFound($"No search results found with searchTerm: {searchTerm}"); 
            }

            return Ok(result);
        }
    }
}
