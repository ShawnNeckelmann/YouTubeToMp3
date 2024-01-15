namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnAudioRipExceptionEventArgs : BaseEventArgs
{
    public string ExceptionMessage { get; }

    public OnAudioRipExceptionEventArgs(Uri url, string exceptionMessage) : base(url)
    {
        ExceptionMessage = exceptionMessage;
    }
}