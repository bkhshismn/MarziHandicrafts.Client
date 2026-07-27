namespace Marzi.Client.Core.Responses;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; }
    public string? RefreshToken { get; set; }
}