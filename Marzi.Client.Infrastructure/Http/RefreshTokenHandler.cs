using Marzi.Client.Core.Authentication;
using Marzi.Client.Infrastructure.Authentication;
using Marzi.Client.Infrastructure.Services;
using Microsoft.JSInterop;
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
            await base.SendAsync(
                request,
                cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        // درخواست‌های authentication نباید وارد refresh شوند.
        if (IsAuthRequest(request))
            return response;

        string? currentToken;

        try
        {
            currentToken =
                await _tokenStorage.GetAccessTokenAsync();
        }
        catch (InvalidOperationException)
        {
            // localStorage/JS هنوز در دسترس نیست.
            return response;
        }
        catch (JSException)
        {
            // JS interop هنوز آماده نیست.
            return response;
        }

        if (string.IsNullOrWhiteSpace(currentToken))
            return response;

        var requestToken =
            request.Headers.Authorization?.Parameter;

        // اگر این درخواست اصلاً با token ارسال نشده،
        // در این مرحله refresh نکن.
        if (string.IsNullOrWhiteSpace(requestToken))
            return response;

        await _coordinator.Lock.WaitAsync(
            cancellationToken);

        try
        {
            // ممکن است درخواست دیگری قبل از ما
            // token را refresh کرده باشد.
            try
            {
                currentToken =
                    await _tokenStorage.GetAccessTokenAsync();
            }
            catch (InvalidOperationException)
            {
                return response;
            }
            catch (JSException)
            {
                return response;
            }

            if (string.IsNullOrWhiteSpace(currentToken))
                return response;

            // اگر token فعلی با token درخواست متفاوت است،
            // یعنی احتمالاً درخواست دیگری قبلاً refresh کرده.
            if (!string.Equals(
                    currentToken,
                    requestToken,
                    StringComparison.Ordinal))
            {
                var retryRequest =
                    await CloneRequestAsync(
                        request,
                        cancellationToken);

                retryRequest.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        currentToken);

                response.Dispose();

                return await base.SendAsync(
                    retryRequest,
                    cancellationToken);
            }

            // فقط وقتی همان token قبلی هنوز فعال است،
            // refresh را انجام بده.
            bool refreshed;

            try
            {
                refreshed =
                    await _authService.RefreshAsync();
            }
            catch (InvalidOperationException)
            {
                return response;
            }
            catch (JSException)
            {
                return response;
            }

            if (!refreshed)
                return response;

            try
            {
                currentToken =
                    await _tokenStorage.GetAccessTokenAsync();
            }
            catch (InvalidOperationException)
            {
                return response;
            }
            catch (JSException)
            {
                return response;
            }

            if (string.IsNullOrWhiteSpace(currentToken))
                return response;

            response.Dispose();

            var retry =
                await CloneRequestAsync(
                    request,
                    cancellationToken);

            retry.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    currentToken);

            return await base.SendAsync(
                retry,
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
            request.RequestUri?.AbsolutePath
            ?? string.Empty;

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