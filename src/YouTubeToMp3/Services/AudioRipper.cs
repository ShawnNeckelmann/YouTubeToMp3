﻿using NReco.VideoConverter;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3.Services;

public class AudioRipper
{
    public void RipAudio(YouTubeData youTubeData, string fileName, string destination)
    {
        var savePath = Path.Combine(destination, $"{youTubeData.FileTitle}.mp3");
        var ffMpeg = new FFMpegConverter();
        ffMpeg.ConvertMedia(fileName, savePath, "mp3");
    }
}