using Spectre.Console;
using YouTubeToMp3.Services.Facade;

namespace YouTubeToMp3.Services;

public class DisplayTable
{
    private const int ColumnStatus = 2;
    private const int ColumnTitle = 1;

    private const int ColumnURL = 0;
    private const int ColumnWidth = 1000;
    private const int RefreshDelay = 100;
    private readonly Table _table;
    private readonly Dictionary<Uri, int> _urlRowNumberDictionary = new();
    private bool _processingComplete;


    public DisplayTable()
    {
        _table = new Table().Centered();

        _table.Border(TableBorder.Heavy);
        _table.AddColumn("[green]URL[/]").Width(ColumnWidth).Centered();
        _table.AddColumn("[fuchsia]Title[/]").Width(ColumnWidth).Centered();
        _table.AddColumn("[blue]Status[/]").Width(ColumnWidth).Centered();

        _table.Caption = new TableTitle("YouTube to MP3");
    }

    public void AddYouTubeVideo(string uri)
    {
        var asUri = new Uri(uri);
        _table.AddRow($"[link]{asUri}[/]", "Pending", "Pending");
        _urlRowNumberDictionary.Add(asUri, _table.Rows.Count - 1);
    }

    public void Complete(Uri url)
    {
        var rowNumber = RowNumber(url);
        _table.UpdateCell(rowNumber, ColumnStatus, "Complete");
    }

    private void Complete()
    {
        _processingComplete = true;
    }

    private void Remove(Uri uri)
    {
        _urlRowNumberDictionary.Remove(uri);
        if (_urlRowNumberDictionary.Count == 0)
        {
            Complete();
        }
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
        _table.UpdateCell(rowNumber, ColumnTitle, data.DisplayTitle);
        _table.UpdateCell(rowNumber, ColumnStatus, status);
    }

    public void SetStatus(Uri url, string status)
    {
        var rowNumber = RowNumber(url);
        _table.UpdateCell(rowNumber, ColumnStatus, status);
    }
}