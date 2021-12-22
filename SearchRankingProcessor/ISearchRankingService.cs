using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRankingProcessor
{
    public interface ISearchRankingService
    {
        SearchRankingResult GetSearchRanking(string host, string searchTerm);
    }
}
