using MediatR;

namespace Application.Commands;

public record DeleteClaimCommand(string Id) : IRequest;
