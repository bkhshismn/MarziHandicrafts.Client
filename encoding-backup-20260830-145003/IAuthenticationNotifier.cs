namespace Marzi.Client.Core.Authentication;

public interface IAuthenticationNotifier
{
    void NotifyAuthenticated(string accessToken);

    void NotifyLoggedOut();

    Task RefreshAsync();
}