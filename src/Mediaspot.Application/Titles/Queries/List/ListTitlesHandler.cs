using Mediaspot.Domain.Titles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mediaspot.Application.Titles.Queries.List;

public sealed class ListTitlesHandler(ITitleRepository repo) : IRequestHandler<ListTitlesQuery, IEnumerable<Title>>
{
    public Task<IEnumerable<Title>> Handle(ListTitlesQuery request, CancellationToken cancellationToken)
    {
        return repo.ListAsync(new(request.PageSize),request.LastId, cancellationToken);
    }
}
