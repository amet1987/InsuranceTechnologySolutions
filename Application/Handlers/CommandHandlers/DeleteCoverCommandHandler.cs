using Application.Commands;
using Application.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using Shared.Constants;
using Shared.Exceptions;

namespace Application.Handlers.CommandHandlers;

public class DeleteCoverCommandHandler : IRequestHandler<DeleteCoverCommand>
{
    private readonly ICoverRepository _coverRepository;
    private readonly IClaimRepository _claimRepository;
    private readonly IBus _bus;
    private readonly ILogger<DeleteCoverCommandHandler> _logger;

    public DeleteCoverCommandHandler(ICoverRepository coverRepository, IClaimRepository claimRepository, IBus bus, ILogger<DeleteCoverCommandHandler> logger)
    {
        _coverRepository = coverRepository;
        _claimRepository = claimRepository;
        _bus = bus;
        _logger = logger;
    }
    public async Task Handle(DeleteCoverCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Cover with Id: {Id}", request.Id);

        var cover = await _coverRepository.GetAsync(request.Id);

        if (cover is null)
        {
            _logger.LogError("Cover not found with Id: {Id}", request.Id);

            throw new KeyNotFoundException($"Cover not found with Id: {request.Id}");
        }

        var claims = await _claimRepository.GetByCoverIdAsync(request.Id);

        if (claims.Count > 0)
        {
            _logger.LogError("Cover with Id: {Id} can't be deleted because it is currently related to existing Claim", request.Id);

            throw new ValidationException($"Cover with Id: {request.Id} can't be deleted because it is currently related to existing Claim");
        }

        await _coverRepository.DeleteAsync(cover);

        // Publish message for auditing
        await CreateAudit(cover.Id);
    }

    private async Task CreateAudit(string Id)
    {
        await _bus.Publish(new CoverCreatedNotification(Id, SharedConstants.HttpDelete));
    }
}
