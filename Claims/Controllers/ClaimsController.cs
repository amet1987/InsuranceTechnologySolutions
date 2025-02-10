using Application.Commands;
using Application.Models.Dto;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace Claims.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClaimsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Claims")]
        [ProducesResponseType(typeof(List<ClaimDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClaimDto>>> GetAsync()
        {
            var results = await _mediator.Send(new GetClaimsQuery());
            return Ok(results);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Claims by id")]
        [ProducesResponseType(typeof(ClaimDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync(string id)
        {
            var result = await _mediator.Send(new GetClaimQuery(id));
            return result is not null ? Ok(result): NotFound();
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new Claim")]
        [ProducesResponseType(typeof(ClaimDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClaimDto>> CreateAsync(CreateClaimCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete Claim by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            await _mediator.Send(new DeleteClaimCommand(id));
            return Ok();
        }
    }
}
