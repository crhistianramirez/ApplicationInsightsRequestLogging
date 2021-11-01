using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Azureblue.ApplicationInsights.RequestLogging
{
    public class RequestLoggerService : IRequestLoggerService
    {
        private readonly RequestLoggerOptions options;
        private readonly IReader reader;

        public RequestLoggerService(RequestLoggerOptions options, IReader reader)
        {
            this.options = options;
            this.reader = reader;
        }

        public async Task AddDataAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var contentType = context.Request.ContentType;
            var canRead = context.Request.Body.CanRead;
            var path = context.Request.Path;

            if
            (
                canRead &&
                this.options.HttpVerbs.Contains(method) &&
                this.options.ContentType == contentType &&
                this.options.Path == path
            )
            {
                var requestBody = await this.reader.ReadAsync(context, this.options.MaxSize, this.options.CutOffText);

                // Write request body to App Insights
                var requestTelemetry = context.Features.Get<RequestTelemetry>();
                requestTelemetry?.Properties.Add(options.PropertyKey, requestBody);
            }
        }
    }
}
