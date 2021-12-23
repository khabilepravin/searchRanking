using FluentAssertions;
using Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SearchRankingProcessor.Tests
{
    public class SearchServiceTests
    {        
        [Fact]
        public async void Search_WithInvalidUrl_ThrowsArgumentException()
        {
            // arrange            
            var searchService = new SearchService(new SearchSettings { SearchLimit = 100, SearchUrl = "google" });

            // act
            Func<Task> act = async () => await searchService.Search("conveyancing");

            // assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid SearchUrl");
        }

        [Fact]
        public async void Search_WithInvalidLimit_ThrowsArgumentException()
        {
            // arrange
            var searchService = new SearchService(new SearchSettings { SearchLimit = 0, SearchUrl = "https://www.google.com.au/" });

            // act
            Func<Task> act = async () => await searchService.Search("conveyancing");

            // assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid SearchLimit");
        }

        [Fact]
        public async void Search_WithInvalidSearchTerm_ThrowsArgumentException()
        {
            // arrange
            var searchService = new SearchService(new SearchSettings { SearchLimit = 100, SearchUrl = "https://www.google.com.au/" });

            // act
            Func<Task> act = async () => await searchService.Search("");

            // assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid SearchTerm");
        }

        [Fact]
        public async void Search_WithValidSearchTerm_ReturnsNonEmptyString()
        {
            // arrange
            var searchService = new SearchService(new SearchSettings { SearchLimit = 100, SearchUrl = "https://www.google.com.au/" });

            // act
            var result = await searchService.Search("conveyancing");

            // assert
            result.Should().NotBeNullOrWhiteSpace();
        }
    }
}
