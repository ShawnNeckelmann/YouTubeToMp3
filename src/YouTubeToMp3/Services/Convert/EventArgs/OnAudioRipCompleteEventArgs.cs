namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnAudioRipCompleteEventArgs : BaseEventArgs
{
    public OnAudioRipCompleteEventArgs(Uri url) : base(url)
    {
    }
}