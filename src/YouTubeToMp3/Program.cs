using Cocona;
using DotNetTools.SharpGrabber;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YouTubeToMp3.Services;
using YouTubeToMp3.Services.Convert;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3;

internal class Program
{
    private static void Main(string[] args)
    {
        CoconaApp.CreateHostBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton(_ =>
                {
                    // Build a multi-grabber
                    var grabber = GrabberBuilder.New()
                        .UseDefaultServices()
                        .AddYouTube()
                        .Build();

                    return grabber;
                });

                services.AddHttpClient();
                services.AddTransient<YouTubeFacade>();
                services.AddTransient<DisplayTable>();
                services.AddTransient<AudioRipper>();
                services.AddTransient<ConvertYouTubeVideoToMp3>();
                services.AddTransient<Application>();
            })
            .Run<Program>(args);
    }

    public async Task Run([FromService] Application app,
        [Argument("A space delimited list of YouTube URLs to rip.")]
        IEnumerable<string> args)
    {
        await app.Run(args);
    }
}