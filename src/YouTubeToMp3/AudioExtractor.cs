using NReco.VideoConverter;
using System;

public class VideoToMp3Converter
{
    public static void ConvertMp4ToMp3(string mp4FilePath, string mp3FilePath)
    {
        try
        {
            var ffMpeg = new FFMpegConverter();
            ffMpeg.ConvertMedia(mp4FilePath, mp3FilePath, "mp3");
            Console.WriteLine("Conversion completed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}