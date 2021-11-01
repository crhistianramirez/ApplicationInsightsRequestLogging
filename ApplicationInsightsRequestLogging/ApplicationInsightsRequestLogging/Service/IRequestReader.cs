using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Azureblue.ApplicationInsights.RequestLogging
{
    public interface IRequestReader
    {
        public Task<string> ReadAsync(HttpContext context, int bytes, string cutOffText);
    }
}
