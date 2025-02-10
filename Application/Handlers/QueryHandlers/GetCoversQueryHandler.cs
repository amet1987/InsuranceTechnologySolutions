using Application.Mappers;
using Application.Models.Dto;
using Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Handlers.QueryHandlers;

public class GetCoversQueryHandler : IRequestHandler<GetCoversQuery, IEnumerable<CoverDto>>
{
    private readonly ICoverRepository _coverRepository;
    private readonly ILogger<GetCoversQueryHandler> _logger;

    public GetCoversQueryHandler(ICoverRepository coverRepository, ILogger<GetCoversQueryHandler> logger)
    {
        _coverRepository = coverRepository;
        _logger = logger;
    }
    public async Task<IEnumerable<CoverDto>> Handle(GetCoversQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all covers");

        var covers = await _coverRepository.GetAsync();

        return covers.MapToDto();
    }
}
