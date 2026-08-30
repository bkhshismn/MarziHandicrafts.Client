using Marzi.Client.Core.Authentication;

namespace Marzi.Client.Infrastructure.Authentication;

public sealed class AuthenticationTokenStore
    : IAuthenticationTokenStore
{
    public string? AccessToken { get; private set; }

    public string? RefreshToken { get; private set; }

    public void SetTokens(
        string accessToken,
        string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public void SetAccessToken(string accessToken)
    {
        AccessToken = accessToken;
    }

    public void Clear()
    {
        AccessToken = null;
        RefreshToken = null;
    }
}