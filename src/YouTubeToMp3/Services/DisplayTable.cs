using Spectre.Console;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3.Services;

public class DisplayTable
{
    private const int ColumnWidth = 1000;
    private const int RefreshDelay = 100;
    private readonly bool _processingComplete = false;
    private readonly Table _table;
    private readonly Dictionary<Uri, int> _urlRowNumberDictionary = new();


    public DisplayTable()
    {
        _table = new Table().Centered();
        
        _table.Border(TableBorder.Heavy);
        _table.AddColumn("[green]URL[/]").Width(ColumnWidth).Centered();
        _table.AddColumn("[fuchsia]Title[/]").Width(ColumnWidth).Centered();
        _table.AddColumn("[blue]Status[/]").Width(ColumnWidth).Centered();
        _table.Expand();

    }

    public void AddYouTubeVideo(string uri)
    {
        var asUri = new Uri(uri);
        _table.AddRow(asUri.ToString(), "Pending", "Pending");
        _urlRowNumberDictionary.Add(asUri, _table.Rows.Count - 1);
    }

    public async Task Render()
    {
        await AnsiConsole.Live(_table).StartAsync(async context =>
        {
            while (!_processingComplete)
            {
                context.Refresh();
                await Task.Delay(RefreshDelay);
            }
        });
    }

    private int RowNumber(Uri url)
    {
        return _urlRowNumberDictionary[url];
    }


    public void SetStatus(Uri url, string status, YouTubeData data)
    {
        var rowNumber = RowNumber(url);
        _table.UpdateCell(rowNumber, 1, data.DisplayTitle);
        _table.UpdateCell(rowNumber, 2, status);
    }

    public void SetStatus(Uri url, string status)
    {
        var rowNumber = RowNumber(url);
        _table.UpdateCell(rowNumber, 2, status);
    }
}