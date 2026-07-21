using Mediaspot.Domain.Titles.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Create;

public sealed record CreateTitleCommand(string Name, TitleType Type, string? Description, DateOnly? ReleaseDate) : IRequest<Guid>;

