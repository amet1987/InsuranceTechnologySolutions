using Application.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Persistence.Entities.Auditing;
using Persistence.Interfaces.Auditing;

namespace Application.Consumers;

public class CoverAuditConsumer : IConsumer<CoverCreatedNotification>
{
    private readonly ICoverAuditRepository _coverAuditRepository;
    private readonly ILogger<CoverAuditConsumer> _logger;

    public CoverAuditConsumer(ICoverAuditRepository coverAuditRepository, ILogger<CoverAuditConsumer> logger)
    {
        _coverAuditRepository = coverAuditRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CoverCreatedNotification> context)
    {
        _logger.LogInformation("Creating audit for Cover with Id:{Id}", context.Message.CoverId);

        var entity = new CoverAudit
        {
            CoverId = context.Message.CoverId,
            HttpRequestType = context.Message.HttpRequestType
        };

        await _coverAuditRepository.CreateAsync(entity);
    }
}
