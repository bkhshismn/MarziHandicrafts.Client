using Marzi.Client.Core.Models.Courses;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface ICourseApi
{
    // ============================================================
    // Courses
    // ============================================================

    [Get("/api/courses")]
    Task<ApiResponse<List<CourseDto>>> GetAllAsync();

    [Get("/api/courses/published")]
    Task<ApiResponse<List<CourseDto>>> GetPublishedAsync();

    [Get("/api/courses/{id}")]
    Task<ApiResponse<CourseDto>> GetByIdAsync(
        Guid id);


    // ============================================================
    // Admin - Course Sections
    // ============================================================

    [Get("/api/admin/courses/{courseId}/sections")]
    Task<ApiResponse<List<CourseSectionDto>>>
        GetSectionsAsync(Guid courseId);

    [Post("/api/admin/courses/{courseId}/sections")]
    Task<ApiResponse<CourseSectionDto>>
        CreateSectionAsync(
            Guid courseId,
            [Body] CourseSectionCreateRequest request);


    // ============================================================
    // Admin - Course Lessons
    // ============================================================

    [Get("/api/admin/sections/{sectionId}/lessons")]
    Task<ApiResponse<List<CourseLessonDto>>>
        GetLessonsAsync(Guid sectionId);

    [Post("/api/admin/sections/{sectionId}/lessons")]
    Task<ApiResponse<CourseLessonDto>>
        CreateLessonAsync(
            Guid sectionId,
            [Body] CourseLessonCreateRequest request);
}


// ================================================================
// Requests
// ================================================================

public sealed class CourseSectionCreateRequest
{
    public string Title { get; set; } = string.Empty;

    public int OrderIndex { get; set; }
}


public sealed class CourseLessonCreateRequest
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DurationSeconds { get; set; }

    public bool IsFreePreview { get; set; }

    public int OrderIndex { get; set; }
}