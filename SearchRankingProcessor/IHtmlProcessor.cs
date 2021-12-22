using System;
using System.Collections.Generic;

namespace SearchRankingHtmlProcessor
{
    public interface IHtmlProcessor
    {
        List<Uri> FetchUrlsFromSearchResult(string cleanerInputHtml);
    }
}
