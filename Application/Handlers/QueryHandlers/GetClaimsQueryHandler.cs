using Application.Mappers;
using Application.Models.Dto;
using Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Handlers.QueryHandlers;

public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<ClaimDto>>
{
    private readonly IClaimRepository _claimRepository;
    private readonly ILogger<GetClaimsQueryHandler> _logger;

    public GetClaimsQueryHandler(IClaimRepository claimRepositor, ILogger<GetClaimsQueryHandler> logger)
    {
        _claimRepository = claimRepositor;
        _logger = logger;
    }
    public async Task<IEnumerable<ClaimDto>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all claims");

        var claims = await _claimRepository.GetAsync();

        return claims.MapToDto();
    }
}
