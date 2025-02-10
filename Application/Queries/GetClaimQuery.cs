using Application.Models.Dto;
using MediatR;

namespace Application.Queries;

public record GetClaimQuery(string Id) : IRequest<ClaimDto?>;
