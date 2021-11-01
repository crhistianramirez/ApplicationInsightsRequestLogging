using Azureblue.ApplicationInsights.RequestLogging.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationInsightsRequestLoggingTests
{
    public class RequestReaderTests
    {
        [Fact]
        public async Task Should_Ready_Body_From_ContextAsync_And_Reset_Stream()
        {
            // Arrange   
            var reader = new RequestReader();
            var expected = "My Test Body";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(expected));

            var context = new DefaultHttpContext();
            context.Request.Body = stream;
            context.Request.ContentLength = stream.Length;

            // Act
            var requestBody = await reader.ReadAsync(context, 0, string.Empty);

            // Assert
            requestBody.Should().Be(expected);
            context.Request.Body.Position.Should().Be(0);
        }

        [Theory]
        [InlineData("Test Body", 0, "Cut", "Test Body")]
        [InlineData("Test Body", 4, "Cut", "TestCut")]
        public async Task Should_Ready_Body_From_ContextAsync_And_Shorten(string body, int bytes, string cutOfText, string expected)
        {
            // Arrange   
            var reader = new RequestReader();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(body));

            var context = new DefaultHttpContext();
            context.Request.Body = stream;
            context.Request.ContentLength = stream.Length;

            // Act
            var requestBody = await reader.ReadAsync(context, bytes, cutOfText);

            // Assert
            requestBody.Should().Be(expected);
            context.Request.Body.Position.Should().Be(0);
        }
    }
}
