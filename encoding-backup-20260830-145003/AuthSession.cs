namespace Marzi.Client.Web.Authentication;

public sealed class AuthSession
{
    public string AccessToken { get; init; } = "";
    public string RefreshToken { get; init; } = "";

    // اگر API شما expires_in یا exp برمی‌گرداند می‌توانیم بعداً اضافه کنیم
    // public DateTimeOffset? ExpiresAt { get; init; }
}
