using FluentAssertions;
using System;
using System.Net.Http;
using Xunit;

namespace SearchRankingHtmlProcessor.Tests
{
    public class ProcessSearchRankingHtmlTests
    {
        [Fact]
        public void FetchSearchResults_WithInvalidHtml_ThrowsArgumentException()
        {
            // arrange
            var processSearchRanking = new ProcessSearchRankingHtml();

            // act
            Action act = () => processSearchRanking.FetchSearchResults(null);

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid html input");
        }

        [Fact]
        public async void FetchSearchResults_WithValidHtml_ReturnsNonEmptyList()
        {
            // arrange
            var processSearchRanking = new ProcessSearchRankingHtml();            
            var httpClient = new HttpClient();
            var request = await httpClient.GetAsync("https://www.google.com.au/search?num=100&q=conveyancing+software");
            var response = await request.Content.ReadAsStringAsync();            

            // act
            var result = processSearchRanking.FetchSearchResults(response);

            // assert
            result.Should().NotBeEmpty();
        }

    }
}
