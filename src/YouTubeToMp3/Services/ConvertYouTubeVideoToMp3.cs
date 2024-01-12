using Microsoft.Extensions.Logging;
using Spectre.Console;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3.Services;

public class ConvertYouTubeVideoToMp3
{
    private readonly AudioRipper _audioRipper;
    private readonly ILogger<ConvertYouTubeVideoToMp3> _logger;
    private readonly YouTubeFacade _youTubeFacade;
    private ProgressTask _downloadProgressTask;

    public ConvertYouTubeVideoToMp3(
        ILogger<ConvertYouTubeVideoToMp3> logger,
        YouTubeFacade youTubeFacade, AudioRipper audioRipper)
    {
        _logger = logger;
        _youTubeFacade = youTubeFacade;
        _audioRipper = audioRipper;
    }

    public async Task Convert(string args)
    {
        var uri = new Uri(args);
        var data = await _youTubeFacade.GetYouTubeData(uri);
        var file = string.Empty;

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                _downloadProgressTask = ctx.AddTask($"[green]Downloading {data.DisplayTitle} [/]");
                file = await _youTubeFacade.DownloadYouTube(data,
                    (totalDownloadSize, totalBytesRead, progressPercentage, authTitle) =>
                        ProgressReport(progressPercentage));
                while (!ctx.IsFinished) Thread.Sleep(TimeSpan.FromSeconds(1));
            });

        AnsiConsole.Status()
            .Start("Ripping audio for {0}...", context =>
            {
                context.Spinner(Spinner.Known.Star2);
                _audioRipper.RipAudio(data, file, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            });
    }

    private void ProgressReport(double? progressPercentage)
    {
        if (progressPercentage.HasValue)
        {
            _downloadProgressTask.Value = progressPercentage.Value;
        }

        if (progressPercentage >= 100)
        {
            _downloadProgressTask.StopTask();
        }
    }
}