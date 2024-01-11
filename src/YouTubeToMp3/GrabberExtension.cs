using DotNetTools.SharpGrabber;
using DotNetTools.SharpGrabber.Grabbed;

namespace YouTubeToMp3SharpGrabber;

public static class GrabberExtension
{
    public static async Task DownloadAndRipYouTubeVideo(this IMultiGrabber grabber, Uri uriYouTubeVideo)
    {
        // Grab from the URL24

        var result = await grabber.GrabAsync(uriYouTubeVideo);

        var objects = result.Resources.Select(grabbed => grabbed as dynamic).ToList();
        var authObj = objects.FirstOrDefault(d => d.Author is not null);
        var author = "Unknown";

        if (authObj is not null)
        {
            author = authObj.Author;
        }

        // Process the result
        var videos = result.Resources<GrabbedMedia>();
        var highestQuality =
            videos.OrderByDescending(v => v.Resolution).Last(); // Replace 'Resolution' with the actual property

        var highestQualityOriginalUri = highestQuality.ResourceUri;
        var authTitle = $"{author} - {result.Title}";

        // Downloading the video
        Console.WriteLine($"Downloading {authTitle}...");

        var tempFileName = Path.GetTempFileName();

        using (var client = new HttpClient())
        {
            var progressReporter =
                new Progress<(long? totalDownloadSize, long totalBytesRead, double? progressPercentage)>(progress =>
                {
                    ReportProgress(progress.totalDownloadSize, progress.totalBytesRead,
                        progress.progressPercentage, authTitle);
                });


            client.Timeout = TimeSpan.FromHours(24);
            var videoBytes = await client.DownloadFileWithProgressAsync(highestQualityOriginalUri, progressReporter);
            //var videoBytes = await client.GetByteArrayAsync(highestQualityOriginalUri);
            Console.WriteLine($"Finished downloading {authTitle}.");
            await File.WriteAllBytesAsync(tempFileName, videoBytes);
        }

        var musicFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        var savePath = Path.Combine(musicFolderPath, $"{author} - {result.SanitizedFileName()}.mp3");

        Console.WriteLine($"Converting {authTitle} to MP3...");
        VideoToMp3Converter.ConvertMp4ToMp3(tempFileName, savePath);
        Console.WriteLine($"Finished converting {authTitle}.");

        File.Delete(tempFileName);
    }

    private static void ReportProgress(long? totalDownloadSize, long totalBytesRead, double? progressPercentage,
        string authTitle)
    {
        Console.WriteLine(
            $"{authTitle}: Downloaded {totalBytesRead} of {totalDownloadSize} bytes. Progress: {progressPercentage:0.00}%");
    }
}