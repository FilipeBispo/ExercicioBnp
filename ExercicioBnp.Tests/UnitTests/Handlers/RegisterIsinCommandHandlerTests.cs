using Xunit;
using Moq;
using ExercicioBnp.Commands.Handler;
using ExercicioBnp.Infrastructure.Interfaces;
using ExercicioBnp.Services;
using ExercicioBnp.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Collections.Generic;
using ExercicioBnp.Model;
using ExercicioBnp.Commands;
using Microsoft.Extensions.Options;
using ExercicioBnp.Exceptions;

namespace ExercicioBnp.Tests.UnitTests.Handlers
{
    public class RegisterIsinCommandHandlerTests
    {
        private readonly Mock<IIsinRepository> _mockIsinRepository;
        private readonly Mock<IExternalPriceService> _mockPriceService;


        public RegisterIsinCommandHandlerTests()
        {
            _mockIsinRepository = new Mock<IIsinRepository>();
            _mockPriceService = new Mock<IExternalPriceService>();
        }

        [Fact]
        public async Task Handle_WhenIsinDoesNotExist_ShouldRegisterNewIsin()
        {
            // Arrange
            string testIsinIdentifier = "TEST12345678";
            _mockIsinRepository.Setup(repo => repo.GetByIsinIdentifierAsync(testIsinIdentifier)).ReturnsAsync((Isin)null);
            _mockPriceService.Setup(service => service.GetPriceForIsin(testIsinIdentifier)).ReturnsAsync(100.0m);

            var handler = new RegisterIsinCommandHandler(
                _mockIsinRepository.Object,
                _mockPriceService.Object
            );

            var request = new RegisterIsinCommand
            {
                IsinIdentifierList = new List<string> { testIsinIdentifier }
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockIsinRepository.Verify(repo => repo.GetByIsinIdentifierAsync(testIsinIdentifier), Times.Once);
            _mockPriceService.Verify(service => service.GetPriceForIsin(testIsinIdentifier), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_WhenIsinIsInvalid_ShouldNotRegisterNewIsin()
        {
            // Arrange
            string testIsinIdentifier = "INVALIDISIN";

            var handler = new RegisterIsinCommandHandler(
                _mockIsinRepository.Object,
                _mockPriceService.Object
            );

            var request = new RegisterIsinCommand
            {
                IsinIdentifierList = new List<string> { testIsinIdentifier }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidIsinException>(() => handler.Handle(request, CancellationToken.None));
            _mockIsinRepository.Verify(repo => repo.GetByIsinIdentifierAsync(testIsinIdentifier), Times.Never);
            _mockPriceService.Verify(service => service.GetPriceForIsin(testIsinIdentifier), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenIsinAlreadyExists_ShouldNotRegisterNewIsin()
        {
            // Arrange
            string testIsinIdentifier = "EXISTINGISIN";
            _mockIsinRepository.Setup(repo => repo.GetByIsinIdentifierAsync(testIsinIdentifier)).ReturnsAsync(new Isin { Identifier = testIsinIdentifier });

            var handler = new RegisterIsinCommandHandler(
                _mockIsinRepository.Object,
                _mockPriceService.Object
            );

            var request = new RegisterIsinCommand
            {
                IsinIdentifierList = new List<string> { testIsinIdentifier }
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockPriceService.Verify(service => service.GetPriceForIsin(testIsinIdentifier), Times.Never);
            Assert.Equal(Unit.Value, result);
        }
    }
}
