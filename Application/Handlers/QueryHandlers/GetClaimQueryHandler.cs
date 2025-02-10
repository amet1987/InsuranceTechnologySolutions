using Application.Mappers;
using Application.Models.Dto;
using Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Handlers.QueryHandlers;

public class GetClaimQueryHandler : IRequestHandler<GetClaimQuery, ClaimDto?>
{
    private readonly IClaimRepository _claimRepository;
    private readonly ILogger<GetClaimQueryHandler> _logger;

    public GetClaimQueryHandler(IClaimRepository claimRepository, ILogger<GetClaimQueryHandler> logger)
    {
        _claimRepository = claimRepository;
        _logger = logger;
    }
    public async Task<ClaimDto?> Handle(GetClaimQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving data for claim with id {Id}", request.Id);

        var claim = await _claimRepository.GetAsync(request.Id);

        return claim.MapToDto();
    }
}
