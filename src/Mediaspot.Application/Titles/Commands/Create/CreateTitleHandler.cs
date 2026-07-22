using Mediaspot.Application.Common;
using Mediaspot.Application.Titles.Exceptions;
using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Create;

public sealed class CreateTitleHandler(ITitleRepository repo, IUnitOfWork unitOfWork) : IRequestHandler<CreateTitleCommand, Guid>
{
    public async Task<Guid> Handle(CreateTitleCommand request, CancellationToken cancellationToken)
    {
        Title? existing = await repo.GetByNameAsync(request.Name, cancellationToken);

        if (existing != null)
            throw TitleAlreadyExistsException.ForNameConflict(request.Name);

        Title title = Title.Create(name: new(request.Name),
            description: request.Description == null ? null : new(request.Description),
            releaseDate: request.ReleaseDate == null ? null : new(request.ReleaseDate),
            type: request.Type);

        await repo.AddAsync(title, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return title.Id;

    }
}
