namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnDownloadBeginningEventArgs : BaseEventArgs
{
    public OnDownloadBeginningEventArgs(Uri url) : base(url)
    {
    }
}