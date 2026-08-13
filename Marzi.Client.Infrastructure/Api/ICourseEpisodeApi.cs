using Marzi.Client.Core.Models.Courses;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface ICourseEpisodeApi
{
    [Multipart]
    [Post("/api/admin/lessons/{lessonId}/video")]
    Task<ApiResponse<CourseEpisodeDto>> UploadAsync(
        Guid lessonId,
        [Header("Authorization")] string authorization,
        [AliasAs("Title")] string title,
        [AliasAs("Description")] string description,
        [AliasAs("Order")] int order,
        [AliasAs("IsFree")] bool isFree,
        [AliasAs("VideoFile")] StreamPart videoFile);

    [Get("/api/lessons/{lessonId}/episodes")]
    Task<ApiResponse<List<CourseEpisodeDto>>> GetByLessonIdAsync(
        Guid lessonId,
        [Header("Authorization")] string authorization);

    [Get("/api/admin/lessons/{lessonId}/video/{episodeId}/status")]
    Task<ApiResponse<CourseEpisodeDto>> GetStatusAsync(
        Guid lessonId,
        Guid episodeId,
        [Header("Authorization")] string authorization);
}