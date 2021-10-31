using Azureblue.ApplicationInsights.RequestLogging;
using FluentAssertions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationInsightsRequestLoggingTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            // Arrange
            var options = new RequestLoggerOptions
            {
                PropertyKey = "RequestBody",
                HttpVerbs = new[] { "POST" },
                MaxSize = 5,
                ContentType = "text/plain; charset=utf-8",
                Path = "/"
            };

            var client = new TelemetryClient(new TelemetryConfiguration
            {
                TelemetryChannel = new FakeTelemetryChannel(),
                InstrumentationKey = "some key"
            });

            var o = new ApplicationInsightsServiceOptions()
            {
                // ?
            };

            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            //services.AddSingleton(typeof(ITelemetryChannel), new FakeTelemetryChannel());
                            services.AddApplicationInsightsTelemetry(o);
                            services.AddSingleton(client);
                            services.AddRequestLogging(options);
                        })
                        .Configure(app =>
                        {
                            //app.Use(async (c, n) =>
                            //{
                            //    // Set the mock
                            //    c.Features.Set(requestTelemetryMock.Object);
                            //    await n.Invoke();
                            //});
                            app.UseRequestLogging();
                        });
                })
                .StartAsync();

            // Act
            var response = await host.GetTestClient().PostAsync("/", new StringContent("Foo"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    // Taken from https://jrolstad.medium.com/unit-testing-and-microsoft-application-insights-6db0929b39e6
    // https://docs.microsoft.com/en-us/azure/azure-monitor/app/telemetry-channels#what-are-telemetry-channels
    public class FakeTelemetryChannel : ITelemetryChannel
    {
        public ConcurrentBag<ITelemetry> SentTelemtries = new ConcurrentBag<ITelemetry>();

        public bool IsFlushed { get; private set; }
        
        public bool? DeveloperMode { get; set; }
        
        public string EndpointAddress { get; set; }

        public void Send(ITelemetry item) => SentTelemtries.Add(item);

        public void Flush() => IsFlushed = true;

        public void Dispose()  {  }
    }
}
