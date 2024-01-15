namespace YouTubeToMp3.Extensions;

public static class HttpClientExtension
{
    private static async Task<byte[]> DownloadFileWithProgressAsync(this HttpClient httpClient, string requestUri,
        IProgress<(long? totalDownloadSize, long totalBytesRead, double? progressPercentage)> progress)
    {
        try
        {
            using var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri),
                HttpCompletionOption.ResponseHeadersRead);
            var contentLength = response.Content.Headers.ContentLength;

            await using var download = await response.Content.ReadAsStreamAsync();
            const int bufferSize = 8192;
            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = await download.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                totalBytesRead += bytesRead;
                var progressPercentage = (double)totalBytesRead / contentLength * 100;
                progress.Report((contentLength, totalBytesRead, progressPercentage));
            }

            return buffer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<byte[]> DownloadFileWithProgressAsync(this HttpClient httpClient,
        Uri uri,
        Progress<(long? totalDownloadSize, long totalBytesRead, double? progressPercentage)> progressReporter)
    {
        return await httpClient.DownloadFileWithProgressAsync(uri.ToString(), progressReporter);
    }
}