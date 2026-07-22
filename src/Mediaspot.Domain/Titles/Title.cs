using Mediaspot.Domain.Common;
using Mediaspot.Domain.Titles.ValueObjects;

namespace Mediaspot.Domain.Titles;

public sealed class Title : AggregateRoot
{
    public TitleName Name { get; private set; }
    public TitleDescription? Description { get; private set; }
    public ReleaseDate? ReleaseDate { get; private set; }
    public TitleType Type { get; private set; }

    private Title(
        TitleName name,
        TitleDescription? description,
        ReleaseDate? releaseDate,
        TitleType type)
    {
        Name = name;
        Description = description;
        ReleaseDate = releaseDate;
        Type = type;
    }

#pragma warning disable
    public Title()
    {
        
    }
#pragma warning enable

    public static Title Create(
        TitleName name,
        TitleDescription? description,
        ReleaseDate? releaseDate,
        TitleType type)
    {
        return new Title(name, description, releaseDate, type);
    }

    public void Rename(TitleName newName)
    {
        Name = newName;
        // Potential domain event: TitleRenamed
    }

    public void ChangeDescription(TitleDescription newDescription)
    {
        Description = newDescription;
        // Potential domain event: TitleDescriptionChanged
    }

    public void ChangeReleaseDate(ReleaseDate newReleaseDate)
    {
        ReleaseDate = newReleaseDate;
        // Potential domain event: TitleReleaseDateChanged
    }

    public void ChangeType(TitleType newType)
    {
        Type = newType;
        // Potential domain event: TitleTypeChanged
    }
}

