using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnYouTubeDataRetrievedEventArgs : BaseEventArgs
{
    public OnYouTubeDataRetrievedEventArgs(Uri url, YouTubeData data) : base(url)
    {
        Data = data;
    }

    public YouTubeData Data { get; }
}