using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;
using Azureblue.ApplicationInsights.RequestLogging;
using Microsoft.AspNetCore.Hosting;

namespace ApplicationInsightsRequestLoggingTests
{
    public class UnitTest1
    {
        https://gist.github.com/matthiasguentert/d53c9ef0d3a21da2be73d80cfa26081e

        [Fact]
        public async Task Test1Async()
        {
            var requestTelemetry = new RequestTelemetry();

            var options = new RequestLoggerOptions
            {
                PropertyKey = "RequestBody",
                HttpVerbs = new[] { "POST" },
                MaxSize = 5,
                ContentType = "text/plain; charset=utf-8",
                Path = "/"
            };

            using var host = await new HostBuilder()
                .ConfigureWebHost(w =>
                {
                    w.UseTestServer().ConfigureServices(s =>
                    {
                        s.AddRequestLogging(options);
                    }).Configure(null);
                })
                .StartAsync();

            //using var host = await new HostBuilder()
            //    .ConfigureWebHost(webBuilder =>
            //{
            //        webBuilder
            //            .UseTestServer()
            //            .ConfigureServices(services =>
            //            {
            //    // services.AddApplicationInsightsTelemetry("foobar");
            //    services.AddRequestLogging(options);
            //            })
            //            .Configure(app => {
            //                app.Use(async (context, next) =>
            //                {
            //                    // set the mock
            //                    context.Features.Set(requestTelemetry);
            //                    await next.Invoke();
            //                });
            //                app.UseRequestBodyLogging();

            //            });
            //    }).StartAsync();
        }
    }
}
