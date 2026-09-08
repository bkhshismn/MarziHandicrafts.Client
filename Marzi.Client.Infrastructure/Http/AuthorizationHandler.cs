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
        Console.WriteLine(
            $"[AUTH] {request.Method} {request.RequestUri}");

        var token =
            await _tokenStorage.GetAccessTokenAsync();

        Console.WriteLine(
            $"[AUTH] Token exists: {!string.IsNullOrWhiteSpace(token)}");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            Console.WriteLine(
                "[AUTH] Bearer token attached.");
        }
        else
        {
            Console.WriteLine(
                "[AUTH] No access token.");
        }

        var response =
            await base.SendAsync(
                request,
                cancellationToken);

        Console.WriteLine(
            $"[AUTH] Response: {(int)response.StatusCode} {response.StatusCode}");

        return response;
    }
}
