using Application.Commands;
using Application.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using Shared.Constants;

namespace Application.Handlers.CommandHandlers;

public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IBus _bus;
    private readonly ILogger<DeleteClaimCommandHandler> _logger;

    public DeleteClaimCommandHandler(IClaimRepository claimRepository, IBus bus, ILogger<DeleteClaimCommandHandler> logger)
    {
        _claimRepository = claimRepository;
        _bus = bus;
        _logger = logger;
    }
    public async Task Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Claim with id {Id}", request.Id);

        var claim = await _claimRepository.GetAsync(request.Id);

        if (claim is null)
        {
            _logger.LogError("Claim not found with Id: {Id}", request.Id);

            throw new KeyNotFoundException($"Claim not found with Id: {request.Id}");
        }

        await _claimRepository.DeleteAsync(claim);

        // Publish message for auditing
        await CreateAudit(claim.Id);
    }

    private async Task CreateAudit(string Id)
    {
        await _bus.Publish(new ClaimCreatedNotification(Id, SharedConstants.HttpDelete));
    }
}
