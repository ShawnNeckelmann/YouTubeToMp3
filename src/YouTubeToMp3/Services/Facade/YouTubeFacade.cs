using DotNetTools.SharpGrabber;
using DotNetTools.SharpGrabber.Grabbed;
using YouTubeToMp3.Extensions;

namespace YouTubeToMp3.Services.Facade;

public class YouTubeFacade
{
    private static readonly List<string> InvalidContainers = ["mp4", "m4a"];
    private readonly IMultiGrabber _grabber;
    private readonly IHttpClientFactory _httpClientFactory;

    public YouTubeFacade(IMultiGrabber grabber, IHttpClientFactory httpClientFactory)
    {
        _grabber = grabber;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> DownloadYouTube(YouTubeData youTubeData,
        Action<double?, YouTubeData> progressReport)
    {
        var tempFileName = Path.GetTempFileName() + "." + youTubeData.Container;
        using var client = _httpClientFactory.CreateClient();

        var progressReporter =
            new Progress<(long? totalDownloadSize, long totalBytesRead, double? progressPercentage)>(progress =>
            {
                progressReport(progress.progressPercentage, youTubeData);
            });

        var videoBytes = await client.DownloadFileWithProgressAsync(youTubeData.ResourceUri, progressReporter);
        await File.WriteAllBytesAsync(tempFileName, videoBytes);
        return tempFileName;
    }

    private static (string Container, Uri Uri) GetAudioUri(GrabResult objects)
    {
        var list = objects.Resources<GrabbedMedia>()
            .WhereHasAudio()
            .Where(media => media.ResourceUri is not null && !InvalidContainers.Contains(media.Container))
            .OrderByDescending(v => v.Resolution)
            .ToList();

        var first = list.First();
        var retval = first.ResourceUri;
        var container = first.Container;
        return (container, retval);
    }

    private static string GetAuthor(IGrabResult result)
    {
        var @default = result.Resources.Select(grabbed => grabbed as dynamic).FirstOrDefault(d => d.Author is not null);
        if (@default is null)
        {
            return "Unknown";
        }

        var retval = @default.Author;
        return retval;
    }

    public async Task<YouTubeData> GetYouTubeData(Uri uriYouTubeVideo)
    {
        var result = await _grabber.GrabAsync(uriYouTubeVideo);
        var author = GetAuthor(result);
        var audioUri = GetAudioUri(result);
        var title = result.Title;

        return new YouTubeData
        {
            Author = author,
            Title = title,
            ResourceUri = audioUri.Uri,
            Container = audioUri.Container
        };
    }
}