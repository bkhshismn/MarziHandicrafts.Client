using Marzi.Client.Infrastructure.Api;
using Refit;

namespace Marzi.Client.Infrastructure.Services;

public sealed class MediaService
{
    private const long MaxImageSize = 10 * 1024 * 1024;

    private readonly IMediaApi _api;

    public MediaService(IMediaApi api)
    {
        _api = api;
    }

    public async Task<string> UploadImageAsync(
        Stream stream,
        string fileName,
        string contentType)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (string.IsNullOrWhiteSpace(fileName))
            throw new InvalidOperationException("نام فایل تصویر معتبر نیست.");

        if (stream.CanSeek && stream.Length > MaxImageSize)
            throw new InvalidOperationException("حجم تصویر نباید بیشتر از ۱۰ مگابایت باشد.");

        var response = await _api.UploadImageAsync(
            new StreamPart(
                stream,
                fileName,
                string.IsNullOrWhiteSpace(contentType)
                    ? "application/octet-stream"
                    : contentType));

        if (!response.IsSuccessStatusCode ||
            string.IsNullOrWhiteSpace(response.Content?.Url))
        {
            throw new InvalidOperationException("بارگذاری تصویر با خطا مواجه شد.");
        }

        return response.Content.Url;
    }
}
