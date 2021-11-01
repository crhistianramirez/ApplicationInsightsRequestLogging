using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Azureblue.ApplicationInsights.RequestLogging.Reader
{
    public class ResponseReader : IReader
    {
        public async Task<string> ReadAsync(HttpContext context, int bytes, string cutOffText)
        {
            using var memoryStream = new MemoryStream();
            memoryStream.Position = 0;
            var reader = new StreamReader(memoryStream);

            string responseBody = string.Empty;
            if (bytes > 0)
            {
                var buffer = new char[bytes];
                _ = await reader.ReadAsync(buffer, 0, bytes);

                responseBody = new string(buffer);

                if (!string.IsNullOrEmpty(cutOffText))
                    responseBody += cutOffText;
            }
            else
            {
                responseBody = await reader.ReadToEndAsync();
            }

            return responseBody;
        }
    }
}
