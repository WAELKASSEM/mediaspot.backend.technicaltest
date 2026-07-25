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
            .HasColumnType("citext");
        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasConversion(
                description => description == null ? null : description.Value,
                value => new TitleDescription(value))
            .HasMaxLength(4000);

        builder.Property(x => x.ReleaseDate)
            .HasConversion(
                releaseDate => releaseDate == null ? null : releaseDate.Value,
                value => new(value));

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
    }
}

