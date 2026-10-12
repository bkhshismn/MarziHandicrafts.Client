using Marzi.Client.Core.Requests;
using Marzi.Client.Core.Responses;
using Marzi.Client.Core.Models;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    Task<ApiResponse<AuthResponse>> LoginAsync([Body] LoginRequest request);

    [Post("/api/auth/register")]
    Task<ApiResponse<AuthResponse>> RegisterAsync([Body] RegisterRequest request);

    [Post("/api/auth/refresh")]
    Task<ApiResponse<AuthResponse>> RefreshAsync([Body] RefreshRequest request);

    [Post("/api/auth/revoke")]
    Task<IApiResponse> RevokeAsync( [Body] RefreshRequest request);

    [Post("/api/auth/logout")]
    Task<ApiResponse<object>> LogoutAsync([Header("Authorization")] string authorization);

    [Get("/api/auth/profile")]
    Task<ApiResponse<ProfileDto>> GetProfileAsync(
        [Header("Authorization")] string authorization);

    [Put("/api/auth/profile")]
    Task<ApiResponse<object>> UpdateProfileAsync(
        [Header("Authorization")] string authorization,
        [Body] UpdateProfileRequest request);

    [Post("/api/auth/change-password")]
    Task<ApiResponse<object>> ChangePasswordAsync(
        [Header("Authorization")] string authorization,
        [Body] ChangePasswordRequest request);
}