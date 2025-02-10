using Application.Models.Dto;
using MediatR;
using Shared.Classes;

namespace Application.Commands;

public record CreateClaimCommand(string CoverId, string Name, ClaimType Type, decimal DamageCost) : IRequest<ClaimDto>;
