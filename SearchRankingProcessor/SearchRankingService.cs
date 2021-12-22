using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRankingProcessor
{
    public class SearchRankingService : ISearchRankingService
    {
        private readonly ISearchService _searchService;
        private readonly ISearchRankingService _searchRankingService;

        public SearchRankingService(ISearchService searchService, ISearchRankingService searchRankingService)
        {
            _searchService = searchService;
            _searchRankingService = searchRankingService;
        }

        public SearchRankingResult GetSearchRanking(string host, string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
