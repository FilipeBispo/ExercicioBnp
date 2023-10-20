using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;

namespace ExercicioBnp.Tests.Mocks
{
    internal class MockHttpClientFactory
    {
        public static HttpClient Create(HttpResponseMessage response)
        {
            var mockHandler = new Mock<DelegatingHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            return new HttpClient(mockHandler.Object);
        }

        public static HttpClient CreateSuccessResponse(decimal price)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(price), Encoding.UTF8, "application/json"),
            };

            return Create(response);
        }

        public static HttpClient CreateNotFoundResponse()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(string.Empty),
            };

            return Create(response);
        }
    }
}
