using System.Threading.Tasks;

namespace SearchRankingProcessor
{
    public interface ISearchService
    {
        Task<string> Search(string searchTerm);
    }
}
