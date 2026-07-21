using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries.GetById;

internal class GetTitleByIdHandler(ITitleRepository repo) : IRequestHandler<GetTitleByIdQuery, Title>
{
    public async Task<Title> Handle(GetTitleByIdQuery request, CancellationToken cancellationToken)
    {
        Title title = await repo.GetAsync(request.Id, cancellationToken) ??
            throw EntityNotFoundException.ForType<Title>(request.Id);
        return title;

    }
}
