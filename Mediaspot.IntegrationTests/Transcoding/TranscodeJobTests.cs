using Mediaspot.Api.Assets.CreateAsset.CreateVideoAsset;
using Mediaspot.Api.Assets.RegisterMediaFile;
using Mediaspot.Api.TranscodeJobs.CreateJob;
using Mediaspot.Api.TranscodeJobs.FailJob;
using Mediaspot.Api.TranscodeJobs.GetJobById;
using Mediaspot.Domain.Transcoding;
using System.Net.Http.Json;
using static AspireSingletonFeature;
namespace Mediaspot.IntegrationTests.Transcoding;

[TestFixture]
public class TranscodeJobTests
{
    static CreateVideoAssetDto BuildRandomVideo()=>new (Codec: "h264",
        Description : "Test video",
        DurationInSeconds : 60, 
        ExternalId : Guid.NewGuid().ToString(),
        FrameRate : 30,
        Height : 720,
        Language : "en", 
        Title : "Test Video", 
        Width : 1280);

    static RegisterMediaFileDto BuildRandomMedia()=>new (Guid.NewGuid().ToString(), 50);

    async Task<(Guid assetId, Guid mediaId)> BuildAssetAndMedia()
    {
        CreateVideoAssetDto videoDto = BuildRandomVideo();
        var createdResponse = await ApiClient.PostAsJsonAsync("assets/videos", videoDto);
        var assetId = await createdResponse.Content.ReadFromJsonAsync<Guid>();
        RegisterMediaFileDto mediaDto = BuildRandomMedia();
        var mediaResponse = await ApiClient.PostAsJsonAsync($"assets/{assetId}/files", mediaDto);
        var mediaId = await mediaResponse.Content.ReadFromJsonAsync<Guid>();

        return (assetId, mediaId);
    }

    [Test]
    public async Task CompleteTranscodeJob_HappyPath()
    {
        var (assetId, mediaId) = await BuildAssetAndMedia();
        var dto = new CreateTranscodeJobDto(assetId, mediaId, "tititit");
        var createTranscodeJobResponse = await ApiClient.PostAsJsonAsync($"transcode-jobs", dto);
        var transcodeJobId = await createTranscodeJobResponse.Content.ReadFromJsonAsync<Guid>();

        _ = await ApiClient.PutAsync($"transcode-jobs/{transcodeJobId}/start",null);
        _ = await ApiClient.PutAsync($"transcode-jobs/{transcodeJobId}/complete", null);

        var getJob = await ApiClient.GetFromJsonAsync<TranscodeJobDto>($"transcode-jobs/{transcodeJobId}");

        Assert.Multiple(() =>
        {
            Assert.That(getJob!.Status, Is.EqualTo(TranscodeStatus.Succeeded.ToString()));
            Assert.That(getJob.MediaFileId, Is.EqualTo(mediaId));
            Assert.That(getJob.AssetId, Is.EqualTo(assetId));
            Assert.That(getJob.UpdatedAt, Is.Not.Null);
        });
    }

    [Test]
    public async Task FailTranscodeJob_HappPath()
    {
        var (assetId, mediaId) = await BuildAssetAndMedia();
        var dto = new CreateTranscodeJobDto(assetId, mediaId, "tititit");
        var createTranscodeJobResponse = await ApiClient.PostAsJsonAsync($"transcode-jobs", dto);
        var transcodeJobId = await createTranscodeJobResponse.Content.ReadFromJsonAsync<Guid>();

        _ = await ApiClient.PutAsync($"transcode-jobs/{transcodeJobId}/start", null);
        FailTranscodeJobDto failTranscodeJobDto = new("wael is useless");
        _ = await ApiClient.PutAsJsonAsync<FailTranscodeJobDto>($"transcode-jobs/{transcodeJobId}/fail", failTranscodeJobDto);

        var getJob = await ApiClient.GetFromJsonAsync<TranscodeJobDto>($"transcode-jobs/{transcodeJobId}");

        Assert.Multiple(() =>
        {
            Assert.That(getJob!.Status, Is.EqualTo(TranscodeStatus.Failed.ToString()));
            Assert.That(getJob.MediaFileId, Is.EqualTo(mediaId));
            Assert.That(getJob.AssetId, Is.EqualTo(assetId));
            Assert.That(getJob.UpdatedAt, Is.Not.Null);
            Assert.That(getJob.FailureReason, Is.EqualTo(failTranscodeJobDto.FailureReason));
        });

    }

}
