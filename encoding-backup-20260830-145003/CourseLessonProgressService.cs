using Marzi.Client.Core.Models.Courses;
using Marzi.Client.Core.Requests;
using Marzi.Client.Infrastructure.Api;

namespace Marzi.Client.Infrastructure.Services;

public sealed class CourseLessonProgressService
{
    private readonly ICourseLessonProgressApi _api;

    public CourseLessonProgressService(
        ICourseLessonProgressApi api)
    {
        _api = api;
    }

    public async Task<CourseLessonProgressDto?> GetProgressAsync(
        Guid lessonId)
    {
        var response =
            await _api.GetProgressAsync(lessonId);

        return response;
    }

    public async Task<CourseLessonProgressDto?> UpdateProgressAsync(
        Guid lessonId,
        int watchedSeconds,
        bool completed)
    {
        var request = new CourseLessonProgressRequest
        {
            WatchedSeconds = watchedSeconds,
            Completed = completed
        };

        return await _api.UpdateProgressAsync(
            lessonId,
            request);
    }
}