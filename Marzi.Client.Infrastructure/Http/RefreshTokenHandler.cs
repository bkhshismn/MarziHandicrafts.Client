using Marzi.Client.Core.Authentication;
using Marzi.Client.Infrastructure.Authentication;
using Marzi.Client.Infrastructure.Services;
using System.Net;
using System.Net.Http.Headers;

namespace Marzi.Client.Infrastructure.Http;

public sealed class RefreshTokenHandler : DelegatingHandler
{
    private readonly AuthService _authService;
    private readonly ITokenStorage _tokenStorage;

    private readonly TokenRefreshCoordinator _coordinator;
    public RefreshTokenHandler(
    AuthService authService,
    ITokenStorage tokenStorage,
    TokenRefreshCoordinator coordinator)
    {
        _authService = authService;
        _tokenStorage = tokenStorage;
        _coordinator = coordinator;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response =
            await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        // درخواست Auth را وارد چرخه Refresh نکن.
        if (IsAuthRequest(request))
            return response;

        await _coordinator.Lock.WaitAsync(cancellationToken);
        try
        {
            // ممکن است request دیگری قبل از ما
            // token را refresh کرده باشد.
            var currentToken =
                await _tokenStorage.GetAccessTokenAsync();

            var requestToken =
                request.Headers.Authorization?.Parameter;

            if (string.Equals(
                    currentToken,
                    requestToken,
                    StringComparison.Ordinal))
            {
                var refreshed =
                    await _authService.RefreshAsync();

                if (!refreshed)
                    return response;

                currentToken =
                    await _tokenStorage.GetAccessTokenAsync();
            }

            if (string.IsNullOrWhiteSpace(currentToken))
                return response;

            response.Dispose();

            var retryRequest =
                await CloneRequestAsync(
                    request,
                    cancellationToken);

            retryRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    currentToken);

            return await base.SendAsync(
                retryRequest,
                cancellationToken);
        }
        finally
        {
            _coordinator.Lock.Release();
        }
    }

    private static bool IsAuthRequest(
        HttpRequestMessage request)
    {
        var path =
            request.RequestUri?.AbsolutePath ?? string.Empty;

        return path.StartsWith(
            "/api/auth/",
            StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<HttpRequestMessage>
        CloneRequestAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
    {
        var clone =
            new HttpRequestMessage(
                request.Method,
                request.RequestUri)
            {
                Version = request.Version,
                VersionPolicy = request.VersionPolicy
            };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(
                header.Key,
                header.Value);
        }

        if (request.Content is not null)
        {
            var content =
                await request.Content.ReadAsByteArrayAsync(
                    cancellationToken);

            var clonedContent =
                new ByteArrayContent(content);

            foreach (var header in request.Content.Headers)
            {
                clonedContent.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }

            clone.Content = clonedContent;
        }

        return clone;
    }
}