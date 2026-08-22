using Marzi.Client.Core.Models.Courses;
using Marzi.Client.Core.Requests;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface ICourseLessonProgressApi
{
    [Get("/api/lessons/{lessonId}/progress")]
    Task<CourseLessonProgressDto> GetProgressAsync(
        Guid lessonId);

    [Put("/api/lessons/{lessonId}/progress")]
    Task<CourseLessonProgressDto> UpdateProgressAsync(
        Guid lessonId,
        [Body] CourseLessonProgressRequest request);
}