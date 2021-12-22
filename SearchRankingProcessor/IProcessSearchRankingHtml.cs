using Models;
using System.Collections.Generic;

namespace SearchRankingHtmlProcessor
{
    public interface IProcessSearchRankingHtml
    {  
        IEnumerable<SearchRankingResult> FetchSearchResults(string cleanerInputHtml);
    }
}
