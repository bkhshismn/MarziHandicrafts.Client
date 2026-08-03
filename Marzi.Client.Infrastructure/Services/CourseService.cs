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

    public async Task<IReadOnlyList<CourseDto>> GetPublishedAsync()
    {
        var response = await _api.GetPublishedAsync();

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت دوره‌ها با خطا مواجه شد.");
        }

        return response.Content;
    }

    public async Task<CourseDto> GetByIdAsync(Guid id)
    {
        var response = await _api.GetByIdAsync(id);

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دوره مورد نظر یافت نشد.");
        }

        return response.Content;
    }
}