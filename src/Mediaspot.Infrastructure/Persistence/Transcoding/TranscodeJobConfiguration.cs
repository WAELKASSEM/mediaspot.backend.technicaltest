using Mediaspot.Domain.Transcoding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Mediaspot.Infrastructure.Persistence.Transcoding;

internal sealed class TranscodeJobConfiguration : IEntityTypeConfiguration<TranscodeJob>
{
    public void Configure(EntityTypeBuilder<TranscodeJob> b)
    {
        b.HasKey(j => j.Id);
        b.Property(j => j.Preset).IsRequired();
        b.Property(j => j.Status);
        b.HasIndex(j => new { j.AssetId, j.Status });
    }
}
