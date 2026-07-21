using Mediaspot.Domain.Titles;
using Mediaspot.Domain.Titles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mediaspot.Infrastructure.Persistence.Titles;

public sealed class TitleConfiguration : IEntityTypeConfiguration<Title>
{
    public void Configure(EntityTypeBuilder<Title> builder)
    {
        builder.ToTable("Titles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasConversion(
                name => name.Value,
                value => new TitleName(value))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasConversion(
                description => description.Value,
                value => new TitleDescription(value))
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.ReleaseDate)
            .HasConversion(
                releaseDate => releaseDate.Value,
                value => new(value))
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
    }
}

