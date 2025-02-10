using Application.Models.Dto;
using MediatR;

namespace Application.Queries;

public record GetCoversQuery() : IRequest<IEnumerable<CoverDto>>;
