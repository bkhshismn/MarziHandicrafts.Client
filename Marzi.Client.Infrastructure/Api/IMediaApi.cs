using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IMediaApi
{
    [Multipart]
    [Post("/api/media/images")]
    Task<ApiResponse<MediaImageUploadResponse>> UploadImageAsync(
        [AliasAs("file")] StreamPart file);
}

public sealed record MediaImageUploadResponse(string Url);
