using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IUsersApi
{
    [Get("/api/users")]
    Task<ApiResponse<ManagedUsersResponse>> ListAsync();
    [Post("/api/users/managed")]
    Task<ApiResponse<ManagedUserDto>> CreateAsync([Body] CreateManagedUserRequest request);
    [Put("/api/users/{id}/categories")]
    Task<IApiResponse> SetCategoriesAsync(Guid id, [Body] SetUserCategoriesRequest request);
    [Put("/api/users/{id}/name")]
    Task<IApiResponse> SetNameAsync(Guid id, [Body] SetUserNameRequest request);
    [Put("/api/users/{id}/role")]
    Task<IApiResponse> SetRoleAsync(Guid id, [Body] SetUserRoleRequest request);
    [Put("/api/users/{id}/active")]
    Task<IApiResponse> SetActiveAsync(Guid id, [Body] SetUserActiveRequest request);
}

public interface ICourseCategoriesApi
{
    [Get("/api/course-categories")]
    Task<ApiResponse<List<string>>> ListAsync();
    [Post("/api/course-categories")]
    Task<ApiResponse<string>> CreateAsync([Body] CreateCourseCategoryRequest request);
    [Put("/api/course-categories")]
    Task<ApiResponse<string>> RenameAsync([Body] RenameCourseCategoryRequest request);
    [Delete("/api/course-categories?name={name}")]
    Task<IApiResponse> DeleteAsync(string name);
}

public sealed class ManagedUsersResponse { public List<ManagedUserDto> Users { get; set; } = []; public List<string> Categories { get; set; } = []; }
public sealed class ManagedUserDto { public Guid Id { get; set; } public string Name { get; set; } = ""; public string Username { get; set; } = ""; public string Role { get; set; } = "User"; public bool IsActive { get; set; } public bool IsApproved { get; set; } = true; public DateTime CreatedAt { get; set; } public List<string> Categories { get; set; } = []; public List<string> IpAddresses { get; set; } = []; }
public sealed class CreateManagedUserRequest { public string Username { get; set; } = ""; public string Name { get; set; } = ""; public string Password { get; set; } = ""; public string Role { get; set; } = "User"; }
public sealed class SetUserCategoriesRequest { public List<string> Categories { get; set; } = []; }
public sealed class SetUserNameRequest { public string Name { get; set; } = ""; }
public sealed class SetUserRoleRequest { public string Role { get; set; } = "User"; }
public sealed class SetUserActiveRequest { public bool IsActive { get; set; } }
public sealed class CreateCourseCategoryRequest { public string Name { get; set; } = ""; }
public sealed class RenameCourseCategoryRequest { public string OldName { get; set; } = ""; public string NewName { get; set; } = ""; }
