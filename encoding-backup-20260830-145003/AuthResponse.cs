namespace Marzi.Client.Core.Responses;

public sealed class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; }
}