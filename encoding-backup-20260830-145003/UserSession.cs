namespace Marzi.Client.Core.Authentication;

public class UserSession
{
    public bool IsAuthenticated { get; set; }

    public string Token { get; set; } = "";

    public string? RefreshToken { get; set; }

    public DateTime ExpireAt { get; set; }

    public string Phone { get; set; } = "";
}