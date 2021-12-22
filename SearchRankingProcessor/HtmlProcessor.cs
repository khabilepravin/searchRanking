using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SearchRankingHtmlProcessor
{
    public class HtmlProcessor : IHtmlProcessor
    {
        public List<Uri> FetchUrlsFromSearchResult(string inputHtml)
        {
            if (string.IsNullOrWhiteSpace(inputHtml))
            {
                throw new ArgumentException("Invalid html input");
            }

            var searchResultUrls = FindSearchResultUrls(inputHtml);
           
            return searchResultUrls;
        }

        // Instead of building a sophisticated html parser, just simply using string lookup for a specific url types in html
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

                        Uri existingUrlDomain = null;
                        if (searchResultUrlElements != null)
                        {
                            existingUrlDomain = (from result in searchResultUrlElements
                                                     where result.Host == uri.Host
                                                     select result).FirstOrDefault<Uri>();
                        }

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
        
    }
}
