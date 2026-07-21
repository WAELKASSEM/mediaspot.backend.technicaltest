using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries.List;

public sealed record ListTitlesQuery(Guid? LastId, int? PageSize) : IRequest<IEnumerable<Title>>;
