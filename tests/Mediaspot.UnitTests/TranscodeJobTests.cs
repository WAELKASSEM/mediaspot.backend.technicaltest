using Mediaspot.Domain.Transcoding;
using Mediaspot.Domain.Transcoding.Exceptions;
using Mediaspot.Domain.Transcoding.ValueObjects;
using Shouldly;

namespace Mediaspot.UnitTests;

public class TranscodeJobTests
{
    [Fact]
    public void MarkRunning_WhenPending_ShouldSetStatusToRunning()
    {
        var job = new TranscodeJob(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Preset("1080p"));

        job.MarkRunning();

        job.Status.ShouldBe(TranscodeStatus.Running);
        job.UpdatedAt.ShouldNotBeNull();
    }

    [Fact]
    public void MarkRunning_WhenNotPending_ShouldThrow()
    {
        var job = new TranscodeJob(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Preset("1080p"));

        job.MarkRunning();

        var action = () => job.MarkRunning();

        action.ShouldThrow<InvalidTranscodeStatusException>();
            
    }

    [Fact]
    public void MarkSucceeded_WhenRunning_ShouldSetStatusToSucceeded()
    {
        var job = new TranscodeJob(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Preset("1080p"));

        job.MarkRunning();

        job.MarkSucceeded();

        job.Status.ShouldBe(TranscodeStatus.Succeeded);
        job.UpdatedAt.ShouldNotBeNull();
    }

    [Fact]
    public void MarkSucceeded_WhenNotRunning_ShouldThrow()
    {
        var job = new TranscodeJob(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Preset("1080p"));

        var action = () => job.MarkSucceeded();

        action.ShouldThrow<InvalidTranscodeStatusException>();
    }

    [Fact]
    public void MarkFailed_WhenRunning_ShouldSetStatusToFailed()
    {
        var job = new TranscodeJob(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Preset("1080p"));

        job.MarkRunning();

        job.MarkFailed("ffmpeg crashed");

        job.Status.ShouldBe(TranscodeStatus.Failed);
        job.FailureReason.ShouldBe("ffmpeg crashed");
        job.UpdatedAt.ShouldNotBeNull();
    }

    [Fact]
    public void MarkFailed_WhenNotRunning_ShouldThrow()
    {
        var job = new TranscodeJob(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Preset("1080p"));

        var action = () => job.MarkFailed("error");

        action.ShouldThrow<InvalidTranscodeStatusException>();
    }
}
