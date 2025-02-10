using Application.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Persistence.Entities.Auditing;
using Persistence.Interfaces.Auditing;

namespace Application.Consumers;

public class ClaimAuditConsumer : IConsumer<ClaimCreatedNotification>
{
    private readonly IClaimAuditRepository _claimAuditRepository;
    private readonly ILogger<ClaimAuditConsumer> _logger;

    public ClaimAuditConsumer(IClaimAuditRepository claimAuditRepository, ILogger<ClaimAuditConsumer> logger)
    {
        _claimAuditRepository = claimAuditRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClaimCreatedNotification> context)
    {
        _logger.LogInformation("Creating audit for Claim with Id:{Id}", context.Message.ClaimId);

        var entity = new ClaimAudit
        {
            ClaimId = context.Message.ClaimId,
            HttpRequestType = context.Message.HttpRequestType
        };

        await _claimAuditRepository.CreateAsync(entity);
    }
}
