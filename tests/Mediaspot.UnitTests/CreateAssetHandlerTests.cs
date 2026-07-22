using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Assets.Commands.Create.Videos;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.VideoAssets;
using Mediaspot.Domain.Assets.VideoAssets.ValueObjects;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests;

public class CreateAssetHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Asset_When_ExternalId_Is_Unique()
    {
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Asset?)null);
        repo.Setup(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var handler = new CreateVideoAssetHandler(repo.Object, uow.Object);
        var cmd = new CreateVideoAssetCommand("ext-unique", "title", "desc", "en",Duration.FromSeconds(3600),Resolution.QHD,60,"H.264");

        var id = await handler.Handle(cmd, CancellationToken.None);

        id.ShouldNotBe(Guid.Empty);
        repo.Verify(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_ExternalId_Exists()
    {
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new VideoAsset("ext", new Metadata("t", null, null), new Duration(TimeSpan.FromHours(2)), Resolution.QHD, 30, "H.264"));
        var handler = new CreateVideoAssetHandler(repo.Object, uow.Object);
        var cmd = new CreateVideoAssetCommand("ext-unique", "title", "desc", "en",Duration.FromSeconds(3600),Resolution.QHD,60,"H.264");

        await Should.ThrowAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
