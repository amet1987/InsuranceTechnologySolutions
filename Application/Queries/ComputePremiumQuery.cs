using MediatR;
using Shared.Classes;

namespace Application.Queries;

public record ComputePremiumQuery(DateTime StartDate, DateTime EndDate, CoverType Type) : IRequest<decimal>;
