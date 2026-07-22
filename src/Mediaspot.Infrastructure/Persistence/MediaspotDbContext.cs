using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.AudioAssets;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.VideoAssets;
using Mediaspot.Domain.Titles;
using Mediaspot.Domain.Transcoding;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence;

public sealed class MediaspotDbContext(DbContextOptions<MediaspotDbContext> options) : DbContext(options)
{
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<VideoAsset> VideoAssets => Set<VideoAsset>();
    public DbSet<AudioAsset> AudioAssets => Set<AudioAsset>();
    public DbSet<TranscodeJob> TranscodeJobs => Set<TranscodeJob>();

    public DbSet<Title> Titles => Set<Title>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MediaspotDbContext).Assembly);
    }
}
