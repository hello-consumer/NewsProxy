using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NewsProxy
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();


            }

            app.Run(async (context) =>
            {

                var clientBuilder = app.ApplicationServices.GetService<IHttpClientFactory>();
                var client = clientBuilder.CreateClient();
                
                string baseUrl = "https://newsapi.org/v2";

                string myApiKey = _configuration["NewsApiKey"];

                string path = context.Request.Path + context.Request.QueryString.ToString();     //top-headlines?country=us

                var apiResponse = await client.GetAsync(baseUrl + path + "&apiKey=" + myApiKey);
                context.Response.Headers.Append("Access-Control-Allow-Origin", "*");    //Open this up for CORS
                await apiResponse.Content.CopyToAsync(context.Response.Body);
            });
        }
    }
}
