using YouTubeToMp3.Services.Convert;
using YouTubeToMp3.Services.Convert.EventArgs;

namespace YouTubeToMp3.Services;

public class Application
{
    private readonly ConvertYouTubeVideoToMp3 _convertYouTubeVideoToMp3;
    private readonly DisplayTable _displayTable;

    public Application(ConvertYouTubeVideoToMp3 convertYouTubeVideoToMp3, DisplayTable displayTable)
    {
        _convertYouTubeVideoToMp3 = convertYouTubeVideoToMp3;
        _displayTable = displayTable;

        _convertYouTubeVideoToMp3.OnDownloadBeginning += OnDownloadBeginning;
        _convertYouTubeVideoToMp3.OnAudioRipComplete += OnAudioRipComplete;
        _convertYouTubeVideoToMp3.OnDownloadComplete += OnDownloadComplete;
        _convertYouTubeVideoToMp3.OnDownloadProgress += OnDownloadProgress;
        _convertYouTubeVideoToMp3.OnRetrievingYouTubeData += OnRetrievingYouTubeData;
        _convertYouTubeVideoToMp3.OnBegginingAudioRip += OnBegginingAudioRip;
        _convertYouTubeVideoToMp3.OnYouTubeDataRetrieved += OnYouTubeDataRetrieved;
        _convertYouTubeVideoToMp3.OnAudioRipException += OnAudioRipException;
    }

    private void OnAudioRipException(object? sender, OnAudioRipExceptionEventArgs e)
    {
        _displayTable.SetException(e.Url, e.ExceptionMessage );
    }

    private void OnAudioRipComplete(object? sender, OnAudioRipCompleteEventArgs e)
    {
        _displayTable.SetStatus(e.Url, "Complete");
    }

    private void OnBegginingAudioRip(object? sender, OnBegginingAudioRipEventArgs e)
    {
        _displayTable.SetStatus(e.Url, "Beginning Audio Rip...");
    }

    private void OnDownloadBeginning(object? sender, OnDownloadBeginningEventArgs e)
    {
        _displayTable.SetStatus(e.Url, "Beginning download...");
    }

    private void OnDownloadComplete(object? sender, OnDownloadCompleteEventArgs e)
    {
        _displayTable.SetStatus(e.Url, "Download complete.");
    }

    private void OnDownloadProgress(object? sender, OnDownloadProgressEventArgs e)
    {
        if (e.ProgressPercentage.HasValue)
        {
            var percentage = (int)e.ProgressPercentage.Value;
            _displayTable.SetStatus(e.Url, $"Downloading {percentage}%");
        }
        else
        {
            _displayTable.SetStatus(e.Url, "Downloading...");
        }
    }

    private void OnRetrievingYouTubeData(object? sender, OnRetrievingYouTubeDataEventArgs e)
    {
        _displayTable.SetStatus(e.Url, "Retrieving YouTube Data...");
    }

    private void OnYouTubeDataRetrieved(object? sender, OnYouTubeDataRetrievedEventArgs e)
    {
        _displayTable.SetStatus(e.Url, "YouTube Data Retrieved", e.Data);
    }


    public async Task Run(IEnumerable<string> urls)
    {
        var enumerable = urls.ToList();
        var tasks = new List<Task>();

        foreach (var url in enumerable)
        {
            _displayTable.AddYouTubeVideo(url);
            tasks.Add(_convertYouTubeVideoToMp3.Convert(url));
        }

        await _displayTable.Render();
        await Task.WhenAll(tasks);
    }
}