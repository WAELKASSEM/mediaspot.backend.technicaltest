using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Application.Titles.Exceptions;
using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Update;

public sealed class UpdateTitleHandler(ITitleRepository repo, IUnitOfWork uow) : IRequestHandler<UpdateTitleCommand, Guid>
{
    public async Task<Guid> Handle(UpdateTitleCommand request, CancellationToken cancellationToken)
    {
        Title title = await repo.GetAsync(id: request.Id, cancellationToken) ?? throw EntityNotFoundException.ForType<Title>(request.Id);

        if (request.Name is not null)
        {
            Title? existing = await repo.GetByNameAsync(request.Name, cancellationToken);
            if (existing is not null && existing.Id != title.Id)
                throw TitleAlreadyExistsException.ForNameConflict(request.Name);
            title.Rename(new(request.Name));
        }


        if (request.Description is not null)
            title.ChangeDescription(new(request.Description));

        if (request.ReleaseDate is not null)
            title.ChangeReleaseDate(new(request.ReleaseDate));

        if (request.Type is not null)
            title.ChangeType(request.Type.Value);

        await repo.UpdateAsync(title, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return title.Id;

    }
}
