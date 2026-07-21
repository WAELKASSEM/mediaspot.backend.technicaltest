using Mediaspot.Application.Events;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets.Events;
using Mediaspot.Domain.Transcoding;
using Moq;

namespace Mediaspot.UnitTests;

public class TranscodeRequestedHandlerTests
{
    [Fact]
    public async Task Handle_Should_Add_TranscodeJob_And_Save()
    {
        var repo = new Mock<ITranscodeJobRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.AddAsync(It.IsAny<TranscodeJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var handler = new TranscodeRequestedHandler(repo.Object, uow.Object);
        var evt = new TranscodeRequested(Guid.NewGuid(), Guid.NewGuid(), "preset");

        await handler.Handle(evt, CancellationToken.None);

        repo.Verify(r => r.AddAsync(It.Is<TranscodeJob>(j => j.AssetId == evt.AssetId && j.MediaFileId == evt.MediaFileId && j.Preset.Value == evt.TargetPreset), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
