using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace SearchRankingAPI.IntegrationTests
{
    internal class TestClientProvider
    {
        public HttpClient Client { get; private set; }
        public TestClientProvider()
        {        
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()
                .UseEnvironment("test")
                .ConfigureAppConfiguration((context, config) => 
                {
                    config
                    .AddJsonFile("appsettings.test.json")
                    .AddEnvironmentVariables();
                }));
            
            Client = server.CreateClient();
        }        
    }
}
