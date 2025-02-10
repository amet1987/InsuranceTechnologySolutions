using Application.Models.Dto;
using MediatR;

namespace Application.Queries;

public record GetCoverQuery(string Id) : IRequest<CoverDto?>;
