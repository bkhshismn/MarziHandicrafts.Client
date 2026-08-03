using Marzi.Client.Core.Models.Courses;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface ICourseApi
{
    [Get("/api/courses")]
    Task<ApiResponse<List<CourseDto>>> GetAllAsync();

    [Get("/api/courses/published")]
    Task<ApiResponse<List<CourseDto>>> GetPublishedAsync();

    [Get("/api/courses/{id}")]
    Task<ApiResponse<CourseDto>> GetByIdAsync(Guid id);
}