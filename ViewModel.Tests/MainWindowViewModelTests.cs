using APIProxy;
using APIProxy.ResposeModels;
using Moq;
using SearchRankingApp.ViewModels;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using System;

namespace ViewModel.Tests
{
    public class MainWindowViewModelTests
    {
        [Fact]
        public void FetchRankingResultsCommandExeucte_WithValidInputParams_PopulatesData()
        {
            // arrange
            var searchRankingAPIMock = new Mock<ISearchRankingAPIProxy>();
            searchRankingAPIMock.Setup(c => c.GetAllRankingResults(It.IsAny<string>()))
                .ReturnsAsync(new List<SearchRankingResult> { 
                    new SearchRankingResult { Host = "leap", Ranking = 0 }, 
                    new SearchRankingResult { Host = "smokeball", Ranking = 1 },
                    new SearchRankingResult { Host = "practiceevolve", Ranking = 2 }
                });

            searchRankingAPIMock.Setup(s => s.GetRankingResultByHost(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new SearchRankingResult { Host = "smokeball", Ranking = 1 });

            var mainWindowViewModel = new MainWindowViewModel(searchRankingAPIMock.Object) {  Domain = "smokeball", SearchTerm="conveyancing software "};

            // act
            mainWindowViewModel.FetchRankingResultsCommandExeucte();

            // assert
            mainWindowViewModel.CurrentRanking.Should().NotBeNullOrWhiteSpace();
            mainWindowViewModel.AllRankings.Should().NotBeNull();
            mainWindowViewModel.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void FetchRankingResultsCommandExeucte_WithServiceException_SetsTheErrorMessage()
        {
            // arrange
            var searchRankingAPIMock = new Mock<ISearchRankingAPIProxy>();
            searchRankingAPIMock.Setup(c => c.GetAllRankingResults(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Unit test excpetion"));

            searchRankingAPIMock.Setup(s => s.GetRankingResultByHost(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Unit test excpetion"));

            var mainWindowViewModel = new MainWindowViewModel(searchRankingAPIMock.Object) { Domain = "smokeball", SearchTerm = "conveyancing software " };

            // act
            mainWindowViewModel.FetchRankingResultsCommandExeucte();

            // assert
            mainWindowViewModel.ErrorMessage.Should().NotBeNullOrWhiteSpace();
            mainWindowViewModel.CurrentRanking.Should().BeNullOrWhiteSpace();
            mainWindowViewModel.AllRankings.Should().BeNull();
        }
    }
}
