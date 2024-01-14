namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnRetrievingYouTubeDataEventArgs : BaseEventArgs
{
    public OnRetrievingYouTubeDataEventArgs(Uri url) : base(url)
    {
    }
}