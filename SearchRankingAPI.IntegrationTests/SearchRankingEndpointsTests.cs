using FluentAssertions;
using System.Net;
using Xunit;

namespace SearchRankingAPI.IntegrationTests
{
    public class SearchRankingEndpointsTests
    {
        [Fact]
        public async void GetRanking_WithValidInputs_ReturnsRanking()
        {
            // arrange
            var client = new TestClientProvider().Client;

            // act
            var response = await client.GetAsync("/api/searchranking?host=smokeball&searchTerm=conveyancing software");

            // assert   
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void GetRanking_WithInValidHost_ReturnsBadRequest()
        {
            // arrange
            var client = new TestClientProvider().Client;

            // act
            var response = await client.GetAsync("/api/searchranking?host=&searchTerm=conveyancing software");

            // assert          
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void GetRanking_WithInValidSearchTerm_ReturnsBadRequest()
        {
            // arrange
            var client = new TestClientProvider().Client;

            // act
            var response = await client.GetAsync("/api/searchranking?host=smokeball&searchTerm=");

            // assert          
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void GetRanking_WithIrrelevantSearchTerm_ReturnsNotFound()
        {
            // arrange
            var client = new TestClientProvider().Client;

            // act
            var response = await client.GetAsync("/api/searchranking?host=smokeball&searchTerm=fargo");

            // assert          
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void GetRankingAll_WithValidInputs_ReturnsRanking()
        {
            // arrange
            var client = new TestClientProvider().Client;

            // act
            var response = await client.GetAsync("/api/searchranking/all?searchTerm=conveyancing software");

            // assert   
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

       

        [Fact]
        public async void GetRankingAll_WithInValidSearchTerm_ReturnsBadRequest()
        {
            // arrange
            var client = new TestClientProvider().Client;

            // act
            var response = await client.GetAsync("/api/searchranking/all?searchTerm=");

            // assert          
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
