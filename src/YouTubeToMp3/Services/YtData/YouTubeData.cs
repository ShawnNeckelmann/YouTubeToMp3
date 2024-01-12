﻿namespace YouTubeToMp3.Services.YtData;

public class YouTubeData
{
    public string Author { get; set; }

    public string FileTitle
    {
        get
        {
            var title = $"{Author} - {Title}";
            var retval = SanitizeFileName(title);
            return retval;
        }
    }

    public Uri ResourceUri { get; set; }

    public string Title { get; set; }

    private static string SanitizeFileName(string filename)
    {
        // Replace invalid characters with an underscore or another preferred character.
        var retval = Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c, '_'));

        return retval;
    }
}