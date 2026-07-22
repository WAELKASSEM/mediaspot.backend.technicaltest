using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.AudioAssets;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.VideoAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mediaspot.Infrastructure.Persistence.Assets;

internal sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> b)
    {
        b.HasKey(a => a.Id);
        b.Property(a => a.ExternalId).IsRequired();
        b.OwnsOne(a => a.Metadata, m =>
        {
            m.Property(x => x.Title).IsRequired();
            m.Property(x => x.Description);
            m.Property(x => x.Language);
        });

        b.Ignore(a => a.MediaFiles);

        b.OwnsMany<MediaFile>("_mediaFiles", mf =>
        {
            mf.WithOwner().HasForeignKey("AssetId");
            mf.ToTable("MediaFiles");

            mf.HasKey(mf => mf.Id);

            mf.Property(x => x.Id)
                .HasConversion(v => v.Value, v => new MediaFileId(v))
                .HasColumnName("MediaFileId")
                .IsRequired();

            mf.Property(mf => mf.Path).HasConversion(v => v.Value, v => new FilePath(v)).HasColumnName("Path").IsRequired();

            mf.Property(mf => mf.Duration).HasConversion(v => v.Value.TotalSeconds, v => new Duration(TimeSpan.FromMilliseconds(v))).HasColumnName("Duration").IsRequired();
        });

        b.Navigation("_mediaFiles").UsePropertyAccessMode(PropertyAccessMode.Field);

        b.Property<bool>("Archived").HasField("<Archived>k__BackingField");
        b.HasIndex(a => a.ExternalId).IsUnique();
    }

}
internal sealed class VideoAssetConfiguration : IEntityTypeConfiguration<VideoAsset>
{
    public void Configure(EntityTypeBuilder<VideoAsset> b)
    {

        b.OwnsOne(
            x => x.Resolution,
            resolution =>
            {
                resolution.Property(x => x.Width)
                    .HasColumnName("Width")
                    .IsRequired();

                resolution.Property(x => x.Height)
                    .HasColumnName("Height")
                    .IsRequired();
            });
        b.Property(x => x.Duration).HasConversion(x => x.Value, v => new Duration(v)).HasColumnName("Duration").IsRequired();

    }
}
internal sealed class AudioAssetConfiguration : IEntityTypeConfiguration<AudioAsset>
{
    public void Configure(EntityTypeBuilder<AudioAsset> b)
    {
        b.Property(x => x.Duration).HasConversion(x => x.Value, v => new Duration(v)).HasColumnName("Duration").IsRequired();
    }
}