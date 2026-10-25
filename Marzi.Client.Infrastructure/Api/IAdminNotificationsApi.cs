using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IAdminNotificationsApi
{
    [Get("/api/admin-notifications")]
    Task<ApiResponse<List<AdminNotificationDto>>> GetUnreadAsync();

    [Post("/api/admin-notifications/read-all")]
    Task<IApiResponse> MarkAllReadAsync();
}

public interface IUserNotificationsApi
{
    [Get("/api/user-notifications")]
    Task<ApiResponse<List<AdminNotificationDto>>> GetUnreadAsync();

    [Post("/api/user-notifications/read-all")]
    Task<IApiResponse> MarkAllReadAsync();
}

public sealed class AdminNotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string TargetUrl { get; set; } = "/admin";
    public DateTime CreatedAt { get; set; }
}
