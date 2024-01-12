using Microsoft.Extensions.Logging;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3.Services;

public class ConvertYouTubeVideoToMp3
{
    private readonly AudioRipper _audioRipper;
    private readonly ILogger<ConvertYouTubeVideoToMp3> _logger;
    private readonly YouTubeFacade _youTubeFacade;
    
    public ConvertYouTubeVideoToMp3(
        ILogger<ConvertYouTubeVideoToMp3> logger,
        YouTubeFacade youTubeFacade,  AudioRipper audioRipper)
    {
        _logger = logger;
        _youTubeFacade = youTubeFacade;
        _audioRipper = audioRipper;
    }

    public async Task Convert(string args)
    {
        var uri = new Uri(args);
        var data = await _youTubeFacade.GetYouTubeData(uri);
        var file = await _youTubeFacade.DownloadYouTube(data, ProgressReport);
        _audioRipper.RipAudio(data, file, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
    }

    private void ProgressReport(long? totalDownloadSize, long totalBytesRead, double? progressPercentage,
        YouTubeData authTitle)
    {
        _logger.LogInformation(
            string.Format("{0}: Downloaded {1} of {2} bytes. Progress: {3:0.00}%", authTitle.Author, totalBytesRead,
                totalDownloadSize, progressPercentage));
    }
}