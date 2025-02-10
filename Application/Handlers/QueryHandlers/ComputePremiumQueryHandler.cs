using Application.Helpers;
using Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.QueryHandlers;

public class ComputePremiumQueryHandler : IRequestHandler<ComputePremiumQuery, decimal>
{
    private readonly ILogger<ComputePremiumQueryHandler> _logger;

    public ComputePremiumQueryHandler(ILogger<ComputePremiumQueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<decimal> Handle(ComputePremiumQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Calculating Premium for StartDate: {StartDate}, EndDate: {EndDate} and CoverType: {Type}", 
            request.StartDate, request.EndDate, request.Type);

        // Not added data validation because it is not required in documentation for ComputePremium endpoint
        var premium = CoverHelper.ComputePremium(request.StartDate, request.EndDate, request.Type);
        return Task.FromResult(premium);
    }
}
