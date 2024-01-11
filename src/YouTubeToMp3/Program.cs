using DotNetTools.SharpGrabber;
using YouTubeToMp3SharpGrabber;

internal class Program
{
    private static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No URLs passed to convert.");
            return;
        }

        // Build a multi-grabber
        var grabber = GrabberBuilder.New()
            .UseDefaultServices()
            .AddYouTube()
            .Build();

        var ts = args
            .Select(s => new Uri(s))
            .Select(uri => grabber.DownloadAndRipYouTubeVideo(uri));


        await Task.WhenAll(ts);
    }
}