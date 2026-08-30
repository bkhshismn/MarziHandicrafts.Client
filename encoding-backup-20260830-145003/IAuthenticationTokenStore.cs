namespace Marzi.Client.Core.Authentication;

public interface IAuthenticationTokenStore
{
    string? AccessToken { get; }

    string? RefreshToken { get; }

    void SetTokens(
        string accessToken,
        string refreshToken);

    void SetAccessToken(string accessToken);

    void Clear();
}