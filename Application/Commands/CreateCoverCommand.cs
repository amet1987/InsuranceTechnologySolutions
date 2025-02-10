using Application.Models.Dto;
using MediatR;
using Shared.Classes;

namespace Application.Commands;

public record CreateCoverCommand(DateTime StartDate, DateTime EndDate, CoverType Type) : IRequest<CoverDto>;
