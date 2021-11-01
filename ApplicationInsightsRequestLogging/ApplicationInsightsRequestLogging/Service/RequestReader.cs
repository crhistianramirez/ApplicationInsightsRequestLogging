using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Azureblue.ApplicationInsights.RequestLogging.Service
{
    public class RequestReader : IRequestReader
    {
        public async Task<string> ReadAsync(HttpContext context, int bytes, string cutOffText)
        {
            // Ensure the request body can be read multiple times
            context.Request.EnableBuffering();

            // Leave stream open so next middleware can read it
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 512, leaveOpen: true);

            var requestBody = string.Empty;

            if (bytes > 0)
            {
                var buffer = new char[bytes];
                var readBytes = await reader.ReadAsync(buffer, 0, bytes);

                requestBody = new string(buffer);

                if (!string.IsNullOrEmpty(cutOffText))
                    requestBody += cutOffText;
            }
            else
            {
                requestBody = await reader.ReadToEndAsync();
            }

            // Reset stream position, so next middleware can read it
            context.Request.Body.Position = 0;

            return requestBody;
        }
    }
}
