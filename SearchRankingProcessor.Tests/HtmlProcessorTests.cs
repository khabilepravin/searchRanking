using FluentAssertions;
using SearchRankingHtmlProcessor;
using System;
using System.Net.Http;
using Xunit;

namespace SearchRankingProcessor.Tests
{
    public class HtmlProcessorTests
    {
        [Fact]
        public void FetchUrlsFromSearchResult_WithInvalidHtml_ThrowsArgumentException()
        {
            // arrange
            var processSearchRanking = new HtmlProcessor();

            // act
            Action act = () => processSearchRanking.FetchUrlsFromSearchResult(null);

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid html input");
        }

        [Fact]
        public async void FetchUrlsFromSearchResult_WithValidHtml_ReturnsNonEmptyList()
        {
            // arrange
            var processSearchRanking = new HtmlProcessor();            
            var httpClient = new HttpClient();
            var request = await httpClient.GetAsync("https://www.google.com.au/search?num=100&q=conveyancing+software");
            var response = await request.Content.ReadAsStringAsync();            

            // act
            var result = processSearchRanking.FetchUrlsFromSearchResult(response);

            // assert
            result.Should().NotBeEmpty();
        }

    }
}
