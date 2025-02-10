using Application.Mappers;
using Application.Models.Dto;
using Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Handlers.QueryHandlers;

public class GetCoverQueryHandler : IRequestHandler<GetCoverQuery, CoverDto?>
{
    private readonly ICoverRepository _coverRepository;
    private readonly ILogger<GetCoverQueryHandler> _logger;

    public GetCoverQueryHandler(ICoverRepository coverRepository, ILogger<GetCoverQueryHandler> logger)
    {
        _coverRepository = coverRepository;
        _logger = logger;
    }
    public async Task<CoverDto?> Handle(GetCoverQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving data for cover with id {Id}", request.Id);

        var cover = await _coverRepository.GetAsync(request.Id);

        return cover.MapToDto();
    }
}
