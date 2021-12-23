﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using SearchRankingProcessor;
using System.Threading.Tasks;
using System.Linq;

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
        public async Task<ActionResult<SearchRankingResult>> Get([FromQuery]string host, [FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) { return BadRequest("Invalid searchTerm"); }
            if (string.IsNullOrWhiteSpace(host)) { return BadRequest("Invalid host"); }


            var result = await _searchRankingService.GetSearchRanking(host, searchTerm);

            if (result == null) { return NotFound($"No search results found for {host} with searchTerm: {searchTerm}"); }

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<SearchRankingResult>> GetAll([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) { return BadRequest("Invalid searchTerm"); }

            var result = await _searchRankingService.GetAllSearchRankingResults(searchTerm);

            if (result?.Any() == false) { return NotFound($"No search results found with searchTerm: {searchTerm}"); }

            return Ok(result);
        }
    }
}