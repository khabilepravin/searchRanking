using FluentAssertions;
using Microsoft.Extensions.Options;
using Models;
using SearchRankingHtmlProcessor;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SearchRankingProcessor.Tests
{
    public class SearchRankingServiceTests
    {
        IOptions<SearchSettings> _searchSettingsOptions;
        
        public SearchRankingServiceTests()
        {
            _searchSettingsOptions = Options.Create<SearchSettings>(new SearchSettings { SearchLimit = 100, SearchUrl = "https://www.google.com.au/" });
        }

        [Fact]
        public async void GetSearchRanking_WithValidParams_ReturnsRankingResult()
        {
            // arrange            
            var searchService = new SearchService(_searchSettingsOptions);
            var htmlProcessor = new HtmlProcessor();

            var searchRankingService = new SearchRankingService(searchService, htmlProcessor);

            // act
            var result = await searchRankingService.GetSearchRanking("smokeball", "conveyancing software");

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetSearchRanking_WithInvalidHost_ThrowsArgumentException()
        {
            // arrange            
            var searchService = new SearchService(_searchSettingsOptions);
            var htmlProcessor = new HtmlProcessor();

            var searchRankingService = new SearchRankingService(searchService, htmlProcessor);

            // act
            Func<Task> act = async () => await searchRankingService.GetSearchRanking("", "conveyancing software");

            // assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid host");
        }

        [Fact]
        public async void GetSearchRanking_WithInvalidSearchTerm_ThrowsArgumentException()
        {
            // arrange            
            var searchService = new SearchService(_searchSettingsOptions);
            var htmlProcessor = new HtmlProcessor();

            var searchRankingService = new SearchRankingService(searchService, htmlProcessor);

            // act
            Func<Task> act = async () => await searchRankingService.GetSearchRanking("smokeball", "");

            // assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid searchTerm");
        }

        [Fact]
        public async void GetAllSearchRankingResults_WithValidParams_ReturnsRankingResult()
        {
            // arrange            
            var searchService = new SearchService(_searchSettingsOptions);
            var htmlProcessor = new HtmlProcessor();

            var searchRankingService = new SearchRankingService(searchService, htmlProcessor);

            // act
            var result = await searchRankingService.GetAllSearchRankingResults("conveyancing software");

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetAllSearchRankingResults_WithInvalidSearchTerm_ThrowsArgumentException()
        {
            // arrange            
            var searchService = new SearchService(_searchSettingsOptions);
            var htmlProcessor = new HtmlProcessor();

            var searchRankingService = new SearchRankingService(searchService, htmlProcessor);

            // act
            Func<Task> act = async () => await searchRankingService.GetAllSearchRankingResults("");

            // assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid searchTerm");
        }

    }
}
