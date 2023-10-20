using ExercicioBnp.Controllers;
using ExercicioBnp.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ExercicioBnp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;

namespace ExercicioBnp.Tests.UnitTests.Controllers
{
    public class IsinControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<IsinController>> _mockLogger;
        private readonly IsinController _controller;

        public IsinControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<IsinController>>();
            _controller = new IsinController(_mockMediator.Object, _mockLogger.Object);
        }

        [Theory]
        [InlineData(new string[] { "TEST12345678" }, typeof(OkObjectResult), null)]
        [InlineData(new string[] { "INVALID_ISIN" }, null, typeof(InvalidIsinException))]
        public async Task RegisterIsin_GivenIsinList_ShouldReturnExpectedResultOrThrowExpectedException(
            string[] isinList,
            Type expectedResponseType,
            Type expectedExceptionType)
        {
            // Arrange
            if (isinList.Contains("INVALID_ISIN"))
            {
                _mockMediator.Setup(m => m.Send(It.IsAny<RegisterIsinCommand>(), It.IsAny<CancellationToken>()))
                    .Throws(new InvalidIsinException("INVALID_ISIN"));
            }
            else
            {
                _mockMediator.Setup(m => m.Send(It.IsAny<RegisterIsinCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Unit.Value);
            }

            // Act
            try
            {
                var result = await _controller.RegisterIsin(isinList.ToList());

                // Assert
                Assert.Null(expectedExceptionType); // We shouldn't be here if an exception was expected
                Assert.IsType(expectedResponseType, result);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Null(expectedResponseType); // We shouldn't be here if a response was expected
                Assert.IsType(expectedExceptionType, ex);
            }
        }
    }
}
