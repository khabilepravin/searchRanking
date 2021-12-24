using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SearchRankingProcessor;
using System;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using SearchRankingHtmlProcessor;
using Microsoft.Extensions.Options;
using Models;
using SearchRankingAPI.Diagnostics;
using Serilog;

namespace SearchRankingAPI
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<SearchSettings>(Configuration.GetSection("SearchSettings"))
                .AddSingleton<SearchSettings>(searchSetting => searchSetting.GetRequiredService<IOptions<SearchSettings>>().Value);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SearchRankingAPI", Version = "v1" });
            });
            
            services.AddSingleton<ISearchRankingService, SearchRankingService>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<IHtmlProcessor, HtmlProcessor>();

            //services.AddHttpClient<ISearchService, SearchService>()
            // .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            // .AddPolicyHandler(GetRetryPolicy());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SearchRankingAPI v1"));
            }

            app.ConfigureExceptionHandler(logger);
            app.UseHttpsRedirection();
           
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
