using System.Net.Http.Headers;
using Marzi.Client.Core.Authentication;

namespace Marzi.Client.Infrastructure.Http;

public sealed class AuthorizationHandler : DelegatingHandler
{
    private readonly ITokenStorage _tokenStorage;

    public AuthorizationHandler(
        ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token =
            await _tokenStorage.GetAccessTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }

        return await base.SendAsync(
            request,
            cancellationToken);
    }
}