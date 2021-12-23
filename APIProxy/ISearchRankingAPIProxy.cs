using APIProxy.ResposeModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIProxy
{
    public interface ISearchRankingAPIProxy
    {
        Task<SearchRankingResult> GetRankingResultByHost(string host, string searchTerm);
        Task<IEnumerable<SearchRankingResult>> GetAllRankingResults(string searchTerm);
    }
}
