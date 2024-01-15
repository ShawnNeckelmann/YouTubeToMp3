namespace YouTubeToMp3.Services.Convert.EventArgs;

public abstract class BaseEventArgs : System.EventArgs
{
    protected BaseEventArgs(Uri url)
    {
        Url = url;
    }


    public Uri Url { get; set; }
}