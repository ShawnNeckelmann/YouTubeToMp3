namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnBegginingAudioRipEventArgs : BaseEventArgs
{
    public OnBegginingAudioRipEventArgs(Uri url) : base(url)
    {
    }
}