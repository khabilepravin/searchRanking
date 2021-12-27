using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchRankingProcessor
{
    public interface ISearchRankingService
    {
        Task<SearchRankingResult> GetSearchRanking(string domain, string searchTerm);

        Task<IEnumerable<SearchRankingResult>> GetAllSearchRankingResults(string searchTerm);
    }
}
