using Application.Commands;
using Application.Mappers;
using Application.Models;
using Application.Models.Dto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Entities;
using Persistence.Interfaces;
using Shared.Constants;
using Shared.Exceptions;

namespace Application.Handlers.CommandHandlers;

public class CreateClaimCommandHandler : IRequestHandler<CreateClaimCommand, ClaimDto>
{
    private readonly IClaimRepository _claimRepository;
    private readonly ICoverRepository _coverRepository;
    private readonly IBus _bus;
    private readonly ILogger<CreateClaimCommandHandler> _logger;

    private const int exceedValue = 100000;

    public CreateClaimCommandHandler(IClaimRepository claimRepository, ICoverRepository coverRepository,
        IBus bus, ILogger<CreateClaimCommandHandler> logger)
    {
        _claimRepository = claimRepository;
        _coverRepository = coverRepository;
        _bus = bus;
        _logger = logger;
    }
    public async Task<ClaimDto> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var requestedDate = DateTime.UtcNow;
        await ValidateRequest(request, requestedDate);

        _logger.LogInformation("Creating new claim with name {Name}", request.Name);

        var entity = new Claim
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = request.CoverId,
            Name = request.Name,
            Created = requestedDate,
            Type = request.Type,
            DamageCost = request.DamageCost,
        };

        var claim = await _claimRepository.CreateAsync(entity);

        // Publish message for auditing
        await CreateAudit(claim.Id);

        return claim.MapToDto()!;

    }

    private async Task ValidateRequest(CreateClaimCommand request, DateTime requestedDate)
    {
        Dictionary<string, string> errors = new();

        if(request.DamageCost > exceedValue) 
            errors.Add("Error1", $"Value of demage cost is higher than {exceedValue}");

        var cover = await _coverRepository.GetAsync(request.CoverId);

        if (cover is null) 
            errors.Add("Error2", "Incorect cover id");

        if(cover is not null && (cover.StartDate.Date > requestedDate.Date && requestedDate.Date < cover.EndDate.Date))
        {
            errors.Add("Error3", "Created is not within the period of the related Cover");
        }

        HasErros(errors);
    }

    private void HasErros(Dictionary<string, string> errors)
    {
        if (errors.Count > 0)
        {
            _logger.LogError("Validation failed");

            throw new ValidationException("Validation failed")
            {
                Errors = errors
            };
        }
    }

    private async Task CreateAudit(string Id)
    {
        await _bus.Publish(new ClaimCreatedNotification(Id, SharedConstants.HttpPost));
    }
}
