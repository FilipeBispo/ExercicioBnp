using ExercicioBnp.Exceptions;
using ExercicioBnp.Services;
using ExercicioBnp.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System.Threading.Tasks;
using ExercicioBnp.Settings;

namespace ExercicioBnp.Tests.UnitTests.Services
{
    public class ExternalPriceServiceTests
    {
        private readonly Mock<ILogger<ExternalPriceService>> _mockLogger;
        private readonly IOptions<ExternalPriceServiceSettings> _mockSettings;
        private ExternalPriceService _service;

        public ExternalPriceServiceTests()
        {
            _mockLogger = new Mock<ILogger<ExternalPriceService>>();
            _mockSettings = MockExternalPriceServiceSettingsFactory.CreateMockSettings();
        }

        [Fact]
        public async Task GetPriceForIsin_WhenResponseIsSuccess_ShouldReturnPrice()
        {
            // Arrange
            var mockHttpClient = MockHttpClientFactory.CreateSuccessResponse(100.0m);
            _service = new ExternalPriceService(mockHttpClient, _mockLogger.Object, _mockSettings);

            // Act
            var result = await _service.GetPriceForIsin("TEST123");

            // Assert
            Assert.Equal(100.0m, result);
        }

        [Fact]
        public async Task GetPriceForIsin_WhenResponseIsNotFound_ShouldThrowException()
        {
            // Arrange
            var mockHttpClient = MockHttpClientFactory.CreateNotFoundResponse();
            _service = new ExternalPriceService(mockHttpClient, _mockLogger.Object, _mockSettings);

            // Act & Assert
            await Assert.ThrowsAsync<ExternalServiceException>(() => _service.GetPriceForIsin("TEST123"));
        }
    }
}
