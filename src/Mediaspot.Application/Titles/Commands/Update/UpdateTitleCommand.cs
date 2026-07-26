using Mediaspot.Domain.Titles.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Update;

public sealed record UpdateTitleCommand(Guid Id,
    string? Name,
    TitleType? Type,
    string? Description,
    DateOnly? ReleaseDate) : IRequest<Guid>;