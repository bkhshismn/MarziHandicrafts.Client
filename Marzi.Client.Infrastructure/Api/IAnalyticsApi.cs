using Marzi.Client.Core.Models;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IAnalyticsApi
{
    [Post("/api/analytics/page-view")]
    Task<IApiResponse> TrackPageViewAsync([Body] PageViewRequest request);

    [Post("/api/analytics/video-view")]
    Task<IApiResponse> TrackVideoViewAsync([Body] VideoViewRequest request);

    [Get("/api/analytics/summary")]
    Task<ApiResponse<AnalyticsSummaryDto>> GetSummaryAsync();
}

public sealed class PageViewRequest
{
    public string Path { get; set; } = "/";
}

public sealed class VideoViewRequest
{
    public Guid LessonId { get; set; }

    public Guid EpisodeId { get; set; }
}
