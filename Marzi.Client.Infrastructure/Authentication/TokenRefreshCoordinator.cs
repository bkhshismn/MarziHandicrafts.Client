namespace Marzi.Client.Infrastructure.Authentication;

public sealed class TokenRefreshCoordinator
{
    public SemaphoreSlim Lock { get; } = new(1, 1);
}