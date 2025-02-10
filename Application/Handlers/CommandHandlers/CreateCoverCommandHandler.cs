using Application.Commands;
using Application.Helpers;
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

public class CreateCoverCommandHandler : IRequestHandler<CreateCoverCommand, CoverDto>
{
    private readonly ICoverRepository _coverRepository;
    private readonly IBus _bus;
    private readonly ILogger<CreateCoverCommandHandler> _logger;

    private const int insurancePeriod = 12;

    public CreateCoverCommandHandler(ICoverRepository coverRepository, IBus bus, ILogger<CreateCoverCommandHandler> logger)
    {
        _coverRepository = coverRepository;
        _logger = logger;
        _bus = bus;
    }
    public async Task<CoverDto> Handle(CreateCoverCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        _logger.LogInformation("Creating new cover with type {Type}", request.Type);

        var entity = new Cover
        {
            Id = Guid.NewGuid().ToString(),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type,
            Premium = CoverHelper.ComputePremium(request.StartDate, request.EndDate, request.Type)
        };

        var cover = await _coverRepository.CreateAsync(entity);

        // Publish message for auditing
        await CreateAudit(cover.Id);

        return cover.MapToDto()!;
    }

    private void ValidateRequest(CreateCoverCommand request)
    {
        Dictionary<string, string> errors = new();

        if (DateTime.Compare(request.StartDate.Date, DateTime.UtcNow.Date) < 0) 
            errors.Add("Error1", "StartDate is in the past");

        if (DateTime.Compare(request.StartDate.Date, request.EndDate.Date) > 0) 
            errors.Add("Error2", $"StartDate: {request.StartDate} is before EndDate: {request.EndDate}");

        if (DateTime.Compare(request.StartDate.AddYears(1).Date, request.EndDate.Date) < 0) 
            errors.Add("Error3", $"Difference between date is more than {insurancePeriod}");

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
        await _bus.Publish(new CoverCreatedNotification(Id, SharedConstants.HttpPost));
    }
}
