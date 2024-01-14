using YouTubeToMp3.Services.Convert.EventArgs;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3.Services.Convert;

public class ConvertYouTubeVideoToMp3
{
    private readonly AudioRipper _audioRipper;
    private readonly YouTubeFacade _youTubeFacade;


    public ConvertYouTubeVideoToMp3(YouTubeFacade youTubeFacade, AudioRipper audioRipper)
    {
        _youTubeFacade = youTubeFacade;
        _audioRipper = audioRipper;
    }

    public async Task Convert(string args)
    {
        var uri = new Uri(args);

        OnRetrievingYouTubeData.Invoke(this, new OnRetrievingYouTubeDataEventArgs(uri));

        var data = await _youTubeFacade.GetYouTubeData(uri);
        OnYouTubeDataRetrieved.Invoke(this, new OnYouTubeDataRetrievedEventArgs(uri, data));


        var file = await _youTubeFacade.DownloadYouTube(data,
            (totalDownloadSize, totalBytesRead, progressPercentage, authTitle) =>
                OnDownloadProgress.Invoke(this, new OnDownloadProgressEventArgs(uri, progressPercentage)));

        OnDownloadComplete.Invoke(this, new OnDownloadCompleteEventArgs(uri));

        OnBegginingAudioRip.Invoke(this, new OnBegginingAudioRipEventArgs(uri));
        _audioRipper.RipAudio(data, file, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        OnAudioRipComplete.Invoke(this, new OnAudioRipCompleteEventArgs(uri));
    }

    public event EventHandler<OnAudioRipCompleteEventArgs> OnAudioRipComplete;

    public event EventHandler<OnBegginingAudioRipEventArgs> OnBegginingAudioRip;

    public event EventHandler<OnDownloadBeginningEventArgs> OnDownloadBeginning;

    public event EventHandler<OnDownloadCompleteEventArgs> OnDownloadComplete;

    public event EventHandler<OnDownloadProgressEventArgs> OnDownloadProgress;

    public event EventHandler<OnRetrievingYouTubeDataEventArgs> OnRetrievingYouTubeData;

    public event EventHandler<OnYouTubeDataRetrievedEventArgs> OnYouTubeDataRetrieved;
}