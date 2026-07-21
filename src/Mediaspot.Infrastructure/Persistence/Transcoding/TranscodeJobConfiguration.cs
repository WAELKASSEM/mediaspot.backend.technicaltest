using Mediaspot.Domain.Transcoding;
using Mediaspot.Domain.Transcoding.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mediaspot.Infrastructure.Persistence.Transcoding;

internal sealed class TranscodeJobConfiguration : IEntityTypeConfiguration<TranscodeJob>
{
    public void Configure(EntityTypeBuilder<TranscodeJob> b)
    {
        b.HasKey(j => j.Id);
        b.Property(j => j.Preset)
            .HasConversion(
                preset => preset.Value,
                value => new Preset(value))
            .IsRequired();
        b.Property(j => j.Status);
        b.HasIndex(j => new { j.AssetId, j.Status });
    }
}
