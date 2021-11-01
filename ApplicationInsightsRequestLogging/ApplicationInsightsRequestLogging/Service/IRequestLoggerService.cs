using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Azureblue.ApplicationInsights.RequestLogging
{
    public interface IRequestLoggerService
    {
        public Task AddDataAsync(HttpContext context);
    }
}
