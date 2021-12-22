using Models;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace SearchRankingHtmlProcessor
{
    public class ProcessSearchRankingHtml : IProcessSearchRankingHtml
    {
        public IEnumerable<SearchRankingResult> FetchSearchResults(string cleanerInputHtml)
        {
            if (string.IsNullOrWhiteSpace(cleanerInputHtml))
            {
                throw new ArgumentException("Invalid html input");
            }

            var searchResultUrls = FindSearchResultUrls(cleanerInputHtml);
            var searchRankings = BuildRankings(searchResultUrls);
            return searchRankings;
        }

        private List<Uri> FindSearchResultUrls(string inputHtml)
        {
            List<Uri> searchResultUrlElements = null;
            
            int searchResultUrlTagIndex = inputHtml.IndexOf(TagNames.UrlTagToLookFor);
            
            while (searchResultUrlTagIndex >= 0)
            {
                inputHtml = inputHtml.Substring(searchResultUrlTagIndex, (inputHtml.Length - searchResultUrlTagIndex));
                
                var urlSearchResultInstance = inputHtml.Substring(0, (inputHtml.IndexOf(TagNames.AnchorEndTag) + TagNames.AnchorEndTag.Length));

                inputHtml = inputHtml.Remove(inputHtml.IndexOf(TagNames.UrlTagToLookFor), urlSearchResultInstance.Length);

                if (urlSearchResultInstance.Contains(TagNames.ImgTag) == false)
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(urlSearchResultInstance);

                    if (doc.DocumentElement.Attributes?.Count > 0)
                    {
                        var hyperLink = doc.DocumentElement.Attributes[0];
                        var uri = new Uri(hyperLink.Value.Replace(TagNames.UrlPrefix, string.Empty));

                        var existingUrlDomain = (from result in searchResultUrlElements
                                                 where result.Host == uri.Host
                                                 select result).FirstOrDefault<Uri>();

                        if (existingUrlDomain == null)
                        {
                            if(searchResultUrlElements == null) { searchResultUrlElements = new List<Uri>(); }
                            searchResultUrlElements.Add(uri);
                        }
                    }
                }

                searchResultUrlTagIndex = inputHtml.IndexOf(TagNames.UrlTagToLookFor);                                    
            }

            return searchResultUrlElements;
        }
        
        private IEnumerable<SearchRankingResult> BuildRankings(List<Uri> searchResultUrls)
        {
            return from url in searchResultUrls
                   select new SearchRankingResult
                   {
                       Host = url.Host,
                       Ranking = searchResultUrls.IndexOf(url)
                   };
        }
    }
}
