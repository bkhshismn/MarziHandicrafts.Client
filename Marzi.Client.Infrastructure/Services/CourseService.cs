using Marzi.Client.Core.Models.Courses;
using Marzi.Client.Infrastructure.Api;

namespace Marzi.Client.Infrastructure.Services;

public sealed class CourseService
{
    private readonly ICourseApi _api;

    public CourseService(ICourseApi api)
    {
        _api = api;
    }

    // ============================================================
    // Courses
    // ============================================================

    public async Task<IReadOnlyList<CourseDto>> GetPublishedAsync()
    {
        try
        {
            var result = await _api.GetPublishedAsync();

            Console.WriteLine(result.StatusCode);

            if (result.Error != null)
                Console.WriteLine(result.Error);

            if (result.Content != null)
                Console.WriteLine($"Count = {result.Content.Count}");

            return result.Content ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public async Task<CourseDto> GetByIdAsync(Guid id)
    {
        var response =
            await _api.GetByIdAsync(id);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دوره مورد نظر یافت نشد.");
        }

        return response.Content;
    }

    public async Task<IReadOnlyList<CourseDto>> GetAllAsync()
    {
        var response =
            await _api.GetAllAsync();

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت دوره‌ها با خطا مواجه شد.");
        }

        return response.Content;
    }


    // ============================================================
    // Admin - Sections
    // ============================================================

    public async Task<IReadOnlyList<CourseSectionDto>>
        GetSectionsAsync(Guid courseId)
    {
        var response =
            await _api.GetSectionsAsync(courseId);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت بخش‌های دوره با خطا مواجه شد.");
        }

        return response.Content
            .OrderBy(section => section.OrderIndex)
            .ToList();
    }


    public async Task<CourseSectionDto>
        CreateSectionAsync(
            Guid courseId,
            CourseSectionCreateRequest request)
    {
        var response =
            await _api.CreateSectionAsync(
                courseId,
                request);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "ایجاد بخش دوره با خطا مواجه شد.");
        }

        return response.Content;
    }


    // ============================================================
    // Admin - Lessons
    // ============================================================

    public async Task<IReadOnlyList<CourseLessonDto>>
        GetLessonsAsync(Guid sectionId)
    {
        var response =
            await _api.GetLessonsAsync(sectionId);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت درس‌های بخش با خطا مواجه شد.");
        }

        return response.Content
            .OrderBy(lesson => lesson.OrderIndex)
            .ToList();
    }


    public async Task<CourseLessonDto>
        CreateLessonAsync(
            Guid sectionId,
            CourseLessonCreateRequest request)
    {
        var response =
            await _api.CreateLessonAsync(
                sectionId,
                request);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "ایجاد درس با خطا مواجه شد.");
        }

        return response.Content;
    }
}