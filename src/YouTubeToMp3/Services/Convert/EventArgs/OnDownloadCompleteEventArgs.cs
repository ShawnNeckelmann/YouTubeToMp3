namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnDownloadCompleteEventArgs : BaseEventArgs
{
    public OnDownloadCompleteEventArgs(Uri url) : base(url)
    {
    }
}