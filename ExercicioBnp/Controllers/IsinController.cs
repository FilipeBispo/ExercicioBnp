using ExercicioBnp.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExercicioBnp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IsinController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<IsinController> _logger;

        public IsinController(IMediator mediator, ILogger<IsinController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Registers a list of ISIN identifiers.
        /// </summary>
        /// <param name="isinIdentifierList">List of ISIN identifiers.</param>
        /// <returns>Response for the registration operation.</returns>
        /// <response code="200">ISIN identifiers registered successfully.</response>
        /// <response code="400">Bad request. This occurs when one or more ISIN identifiers are invalid or not supported.</response>
        /// <response code="500">Internal server error. This typically means there was an issue processing the request.</response>
        /// <response code="502">Bad gateway. This may indicate a problem with an external service that the API depends on.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]  // Assuming you're returning a string message for bad requests.
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]  // For general internal server errors.
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(string))]  // For bad gateway errors, perhaps due to an external service failure.
        public async Task<IActionResult> RegisterIsin(List<string> isinIdentifierList)
        {
            _logger.LogInformation("RegistringIsin start");
            var command = new RegisterIsinCommand { IsinIdentifierList = isinIdentifierList };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }

}
