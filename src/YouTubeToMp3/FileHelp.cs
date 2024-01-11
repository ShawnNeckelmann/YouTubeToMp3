using DotNetTools.SharpGrabber;

namespace YouTubeToMp3SharpGrabber;

public static class FileHelp
{
    public static string SanitizedFileName(this GrabResult grabResult)
    {
        var title = grabResult.Title;
        var retval = SanitizeFileName(title);
        return retval;
    }

    private static string SanitizeFileName(string filename)
    {
        // Replace invalid characters with an underscore or another preferred character.
        var retval = Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c, '_'));

        return retval;
    }
}