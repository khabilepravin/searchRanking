using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Models;
using SearchRankingHtmlProcessor;
using SearchRankingProcessor;

[assembly: FunctionsStartup(typeof(SearchRanking.ServerlessFunc.Startup))]
namespace SearchRanking.ServerlessFunc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var settings = new SearchSettings { SearchUrl = Environment.GetEnvironmentVariable("SearchUrl"), SearchLimit = Convert.ToInt32(Environment.GetEnvironmentVariable("SearchLimit")) };// Newtonsoft.Json.JsonConvert.DeserializeObject<SearchSettings>(Environment.GetEnvironmentVariable("SearchSettings"));
            builder.Services.AddSingleton<SearchSettings>(settings);
            builder.Services.AddScoped<ISearchService, SearchService>();
            builder.Services.AddScoped<IHtmlProcessor, HtmlProcessor>();
            builder.Services.AddScoped<ISearchRankingService, SearchRankingService>();
        }
    }
}
