using MediatR;

namespace Application.Commands;

public record DeleteCoverCommand(string Id) : IRequest;
