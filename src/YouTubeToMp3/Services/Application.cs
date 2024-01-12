namespace YouTubeToMp3.Services;

public class Application
{
    private readonly ConvertYouTubeVideoToMp3 _convertYouTubeVideoToMp3;

    public Application(ConvertYouTubeVideoToMp3 convertYouTubeVideoToMp3)
    {
        _convertYouTubeVideoToMp3 = convertYouTubeVideoToMp3;
    }

    public async Task Run(IEnumerable<string> args)
    {
        var tasks = args.Select(s => _convertYouTubeVideoToMp3.Convert(s));
        await Task.WhenAll(tasks);
    }
}