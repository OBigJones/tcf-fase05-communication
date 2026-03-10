using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Communication.Api.Contracts.Requests;
using Communication.Api.Contracts.Responses;
using Communication.Api.Mappers;
using Communication.Application.UseCases.Handlers;
using Communication.Application.UseCases.Outputs;

namespace Communication.Api.Controllers
{
    [ApiController]
    [Route("communications")]
    public class CommunicationController : ControllerBase
    {
        private readonly SendCommunicationHandler _handler;
        private readonly ILogger<CommunicationController> _logger;

        public CommunicationController(SendCommunicationHandler handler, ILogger<CommunicationController> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        [HttpPost("test")]
        public async Task<ActionResult<SendCommunicationResponse>> Test([FromBody] SendCommunicationRequest request)
        {
            _logger.LogInformation("Manual test endpoint called for {Email}", request.Email);

            var input = request.ToInput();

            var output = await _handler.HandleAsync(input);

            return Ok(new SendCommunicationResponse { Sent = output.Sent, Message = output.Message });
        }
    }
}
