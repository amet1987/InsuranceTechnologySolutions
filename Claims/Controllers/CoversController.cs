using Application.Commands;
using Application.Models.Dto;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class CoversController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoversController(IMediator mediator)
    {
       _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all Covers")]
    [ProducesResponseType(typeof(List<CoverDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CoverDto>>> GetAsync()
    {
        var results = await _mediator.Send(new GetCoversQuery());
        return Ok(results);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get Cover by id")]
    [ProducesResponseType(typeof(CoverDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAsync(string id)
    {
        var result = await _mediator.Send(new GetCoverQuery(id));
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create new Cover")]
    [ProducesResponseType(typeof(CoverDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CoverDto>> CreateAsync([FromBody] CreateCoverCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete Cover by Id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _mediator.Send(new DeleteCoverCommand(id));
        return Ok();
    }

    [HttpGet("compute")]
    [SwaggerOperation(Summary = "Compute Premium")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<decimal>> ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var result = await _mediator.Send(new ComputePremiumQuery(startDate, endDate, coverType));
        return Ok(result);
    }
}
