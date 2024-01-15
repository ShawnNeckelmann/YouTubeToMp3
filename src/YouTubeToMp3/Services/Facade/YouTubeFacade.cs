﻿using DotNetTools.SharpGrabber;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YouTubeToMp3.Services.Facade;

public class YouTubeFacade
{
    private static readonly List<string> InvalidContainers = ["mp4", "m4a"];
    private readonly IMultiGrabber _grabber;
    private readonly IHttpClientFactory _httpClientFactory;

    public YouTubeFacade(IMultiGrabber grabber, IHttpClientFactory httpClientFactory)
    {
        _grabber = grabber;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> DownloadYouTube(YouTubeData youTubeData,
        Action<double?, YouTubeData> progressReport)
    {
        var tempFileName = Path.GetTempFileName();
        var youtube = new YoutubeClient();
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(youTubeData.ResourceUri.ToString());

        var streamInfo = streamManifest
            .GetAudioOnlyStreams()
            .GetWithHighestBitrate();

        var progressReporter = new Progress<double>(d => progressReport(d, youTubeData));

        await youtube.Videos.Streams.DownloadAsync(streamInfo, tempFileName, progressReporter);

        return tempFileName;
    }

    public async Task<YouTubeData> GetYouTubeData(Uri uriYouTubeVideo)
    {
        var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync(uriYouTubeVideo.ToString());


        return new YouTubeData
        {
            Author = video.Author.ChannelTitle,
            Title = video.Title,
            ResourceUri = new Uri(video.Url)
        };
    }
}