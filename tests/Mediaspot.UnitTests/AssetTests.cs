using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.Events;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.VideoAssets;
using Mediaspot.Domain.Assets.VideoAssets.Events;
using Mediaspot.Domain.Assets.VideoAssets.ValueObjects;
using Shouldly;

namespace Mediaspot.UnitTests;

public class AssetTests
{
    private VideoAsset @default = new VideoAsset("ext", new Metadata("t", null, null), new Duration(TimeSpan.FromHours(2)), Resolution.QHD, 30, "H.264");

    [Fact]
    public void Constructor_Should_Set_Properties_And_Raise_AssetCreated()
    {
        var metadata = new Metadata("title", "desc", "en");
        var asset = new VideoAsset("ext-1", metadata, new Duration(TimeSpan.FromHours(2)), Resolution.QHD, 30, "H.264"); ;
        asset.ExternalId.ShouldBe("ext-1");
        asset.Metadata.ShouldBe(metadata);
        asset.DomainEvents.OfType<VideoAssetCreated>().Any(ac => ac.AssetId == asset.Id).ShouldBeTrue();
    }

    [Fact]
    public void RegisterMediaFile_Should_Add_File_And_Raise_Event()
    {
        var asset = @default;
        var path = new FilePath("/file.mp4");
        var duration = Duration.FromSeconds(10);

        var mf = asset.RegisterMediaFile(path, duration);

        asset.MediaFiles.ShouldContain(mf);
        asset.DomainEvents.OfType<MediaFileRegistered>().Any(reg => reg.AssetId == asset.Id && reg.MediaFileId == mf.Id.Value).ShouldBeTrue();
    }

    [Fact]
    public void UpdateMetadata_Should_Set_Metadata_And_Raise_Event()
    {
        var asset = @default;
        var newMeta = new Metadata("new", "d", "fr");

        asset.UpdateMetadata(newMeta);

        asset.Metadata.ShouldBe(newMeta);
        asset.DomainEvents.OfType<MetadataUpdated>().Any(mu => mu.AssetId == asset.Id).ShouldBeTrue();
    }

    [Fact]
    public void UpdateMetadata_Should_Throw_If_Title_Empty()
    {
        var asset = @default;
        var invalid = new Metadata("", null, null);

        Should.Throw<ArgumentException>(() => asset.UpdateMetadata(invalid));
    }

    [Fact]
    public void Archive_Should_Set_Archived_And_Raise_Event()
    {
        var asset = @default;
        asset.Archive(_ => false);

        asset.Archived.ShouldBeTrue();
        asset.DomainEvents.OfType<AssetArchived>().Any(aa => aa.AssetId == asset.Id).ShouldBeTrue();
    }

    [Fact]
    public void Archive_Should_Throw_If_ActiveJobs()
    {
        var asset = @default;
        Should.Throw<InvalidOperationException>(() => asset.Archive(_ => true));
    }

    [Fact]
    public void Archive_Should_Be_Idempotent()
    {
        var asset = @default;
        asset.Archive(_ => false);
        asset.Archive(_ => false);
        asset.Archived.ShouldBeTrue();
        asset.DomainEvents.OfType<AssetArchived>().Count().ShouldBe(1);
    }
}
