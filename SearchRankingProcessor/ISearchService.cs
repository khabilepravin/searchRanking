using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRankingProcessor
{
    public interface ISearchService
    {
        string Search(string searchCriteria);
    }
}
