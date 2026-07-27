using Marzi.Client.Core.Requests;
using Marzi.Client.Core.Responses;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    Task<ApiResponse<AuthResponse>> Login(LoginRequest request); // استفاده از ApiResponse

    [Post("/api/auth/refresh")]
    Task<ApiResponse<AuthResponse>> Refresh(RefreshRequest request); // استفاده از ApiResponse

    [Post("/api/auth/logout")]
    Task<IApiResponse> Logout(); // یا Task<HttpResponseMessage>
}