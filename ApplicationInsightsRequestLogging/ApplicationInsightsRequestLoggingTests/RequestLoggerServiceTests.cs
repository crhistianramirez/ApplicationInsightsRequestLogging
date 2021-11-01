using Azureblue.ApplicationInsights.RequestLogging;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace ApplicationInsightsRequestLoggingTests
{
    public class RequestLoggerServiceTests
    {
        //[Fact]
        //public async void Should_Only_Read_If_Request_Matches()
        //{
        //    var f = new FakeHttpContext.FakeHttpContext();

        //    var c = new DefaultHttpContext();
        //    var r = c.Request;
        //    r.Path = new PathString("/juhu");

        //    f.S

        //    // Arrange
        //    var reader = new Mock<IRequestReader>();
        //    var options = new RequestLoggerOptions();
        //    var service = new RequestLoggerService(options, reader.Object);

        //    // Act

        //    // Assert
        //    reader.Verify(r => r.ReadAsync(new DefaultHttpContext(), 0, string.Empty), Times.Once());
        //}
    }
}
