using ExercicioBnp.Middleware;
using ExercicioBnp.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System.Text;
using Xunit;

namespace ExercicioBnp.Tests.UnitTests.Middleware
{
    public class ExceptionMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly ExceptionMiddleware _middleware;
        private readonly DefaultHttpContext _httpContext;
        private readonly MemoryStream _stream;
        private readonly StreamWriter _writer;

        public ExceptionMiddlewareTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _middleware = new ExceptionMiddleware(_mockNext.Object);

            _httpContext = new DefaultHttpContext();
            _stream = new MemoryStream();
            _writer = new StreamWriter(_stream);

            _httpContext.Response.Body = _stream;
        }

        [Theory]
        [InlineData(typeof(DatabaseConnectionException), 500)]
        [InlineData(typeof(ExternalServiceException), 502)]
        [InlineData(typeof(InvalidIsinException), 400)]
        [InlineData(typeof(IsinInsertionException), 500)]
        public async Task ExceptionMiddleware_ShouldHandleExceptionsCorrectly(Type exceptionType, int expectedStatusCode)
        {
            var exceptionParameter = "Test Exception"; // Isin ou message
            var exceptionInstance = (Exception)Activator.CreateInstance(exceptionType, exceptionParameter);
            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Throws(exceptionInstance);

            await _middleware.InvokeAsync(_httpContext);

            _writer.Flush();

            var responseBody = Encoding.UTF8.GetString(_stream.ToArray());

            Assert.Equal(expectedStatusCode, _httpContext.Response.StatusCode);
            Assert.Contains("There was an error", responseBody);
            Assert.Contains(exceptionParameter, responseBody);
        }

        [Fact]
        public async Task ExceptionMiddleware_ShouldHandleGeneralExceptionsCorrectly()
        {
            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Throws(new Exception("General Exception"));

            await _middleware.InvokeAsync(_httpContext);

            _writer.Flush();

            var responseBody = Encoding.UTF8.GetString(_stream.ToArray());

            Assert.Equal(500, _httpContext.Response.StatusCode);
            Assert.Equal("An unexpected error occurred.", responseBody);
        }
    }
}
