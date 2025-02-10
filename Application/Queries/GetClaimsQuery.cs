using Application.Models.Dto;
using MediatR;

namespace Application.Queries;

public record GetClaimsQuery() : IRequest<IEnumerable<ClaimDto>>;
