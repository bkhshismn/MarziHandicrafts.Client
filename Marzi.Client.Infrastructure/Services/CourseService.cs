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
        var response = await _api.GetByIdAsync(id);

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
        var response = await _api.GetAllAsync();

        if (!response.IsSuccessStatusCode ||
            response.Content is null)
        {
            throw new InvalidOperationException(
                "دریافت دوره‌ها با خطا مواجه شد.");
        }

        return response.Content;
    }
}