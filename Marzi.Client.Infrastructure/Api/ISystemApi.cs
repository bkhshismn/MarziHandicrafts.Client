using Marzi.Client.Core.Models;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface ISystemApi
{
    [Get("/api/admin/system/status")]
    Task<ApiResponse<SystemStatusDto>> GetStatusAsync();
}
