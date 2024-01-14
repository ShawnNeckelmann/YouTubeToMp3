namespace YouTubeToMp3.Services.Convert.EventArgs;

public class OnDownloadProgressEventArgs : BaseEventArgs
{
    public OnDownloadProgressEventArgs(Uri url, double? progressPercentage) : base(url)
    {
        ProgressPercentage = progressPercentage;
    }

    public double? ProgressPercentage { get; set; }
}