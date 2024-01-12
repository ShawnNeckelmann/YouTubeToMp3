using DotNetTools.SharpGrabber;
using DotNetTools.SharpGrabber.Grabbed;
using YouTubeToMp3.Extensions;

namespace YouTubeToMp3.Services.Facade;

public class YouTubeFacade
{
    private readonly IMultiGrabber _grabber;
    private readonly IHttpClientFactory _httpClientFactory;

    public YouTubeFacade(IMultiGrabber grabber, IHttpClientFactory httpClientFactory)
    {
        _grabber = grabber;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> DownloadYouTube(YouTubeData youTubeData,
        Action<long?, long, double?, YouTubeData> progressReport)
    {
        var tempFileName = Path.GetTempFileName();

        using var client = _httpClientFactory.CreateClient();
        var progressReporter =
            new Progress<(long? totalDownloadSize, long totalBytesRead, double? progressPercentage)>(progress =>
            {
                progressReport(progress.totalDownloadSize, progress.totalBytesRead,
                    progress.progressPercentage, youTubeData);
            });

        var videoBytes = await client.DownloadFileWithProgressAsync(youTubeData.ResourceUri, progressReporter);
        await File.WriteAllBytesAsync(tempFileName, videoBytes);
        return tempFileName;
    }

    public async Task<YouTubeData> GetYouTubeData(Uri uriYouTubeVideo)
    {
        var result = await _grabber.GrabAsync(uriYouTubeVideo);

        var objects = result.Resources.Select(grabbed => grabbed as dynamic).ToList();
        var authObj = objects.FirstOrDefault(d => d.Author is not null);
        var author = "Unknown";

        if (authObj is not null)
        {
            author = authObj.Author;
        }

        var title = result.Title;

        return new YouTubeData
        {
            Author = author,
            Title = title,
            ResourceUri = result.Resources<GrabbedMedia>().OrderByDescending(v => v.Resolution).Last().ResourceUri
        };
    }
}