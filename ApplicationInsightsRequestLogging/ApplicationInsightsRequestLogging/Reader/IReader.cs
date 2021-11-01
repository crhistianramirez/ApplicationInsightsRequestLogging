using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Azureblue.ApplicationInsights.RequestLogging
{
    public interface IReader
    {
        public Task<string> ReadAsync(HttpContext context, int bytes, string cutOffText);
    }
}
