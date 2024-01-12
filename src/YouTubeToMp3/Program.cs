using Cocona;
using DotNetTools.SharpGrabber;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YouTubeToMp3.Services;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3;

internal class Program
{
    private static void Main(string[] args)
    {
        CoconaApp.CreateHostBuilder()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
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
                services.AddTransient<AudioRipper>();
                services.AddTransient<ConvertYouTubeVideoToMp3>();
                services.AddTransient<Application>();
            })
            .Run<Program>(args);
    }

    public async Task Run([FromService] Application app, [FromService] ILogger<Program> loggger,
        [Argument("A space delimited list of YouTube URLs to rip.")]
        IEnumerable<string> args)
    {
        loggger.LogInformation("Received some arguments...");
        await app.Run(args);
    }
}