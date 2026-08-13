using Marzi.Client.Core.Authentication;
using Marzi.Client.Core.Models.Courses;
using Marzi.Client.Infrastructure.Api;
using Refit;

namespace Marzi.Client.Infrastructure.Services;

public sealed class CourseEpisodeService
{
    private readonly ICourseEpisodeApi _api;
    private readonly ITokenStorage _tokenStorage;

    public CourseEpisodeService(
        ICourseEpisodeApi api,
        ITokenStorage tokenStorage)
    {
        _api = api;
        _tokenStorage = tokenStorage;
    }

    // =========================================================
    // Upload Video
    // =========================================================

    public async Task<CourseEpisodeDto> UploadAsync(
        Guid lessonId,
        string title,
        string description,
        int order,
        bool isFree,
        Stream stream,
        string fileName,
        string contentType)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new InvalidOperationException(
                "فایل ویدئو انتخاب نشده است.");
        }

        var accessToken =
            await _tokenStorage.GetAccessTokenAsync();

        Console.WriteLine(
            $"[EPISODE UPLOAD] AccessToken exists: " +
            $"{!string.IsNullOrWhiteSpace(accessToken)}");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "توکن ورود پیدا نشد. لطفاً دوباره وارد شوید.");
        }

        var authorization =
            $"Bearer {accessToken}";

        var filePart =
            new StreamPart(
                stream,
                fileName,
                string.IsNullOrWhiteSpace(contentType)
                    ? "application/octet-stream"
                    : contentType);

        try
        {
            Console.WriteLine(
                "[EPISODE UPLOAD] Sending request...");

            var response =
                await _api.UploadAsync(
                    lessonId,
                    authorization,
                    title,
                    description,
                    order,
                    isFree,
                    filePart);

            Console.WriteLine(
                $"[EPISODE UPLOAD] Status: " +
                $"{(int)response.StatusCode} " +
                $"{response.StatusCode}");

            if (!response.IsSuccessStatusCode ||
                response.Content is null)
            {
                throw new InvalidOperationException(
                    $"آپلود ویدئو ناموفق بود. " +
                    $"کد وضعیت: {(int)response.StatusCode}");
            }

            Console.WriteLine(
                "[EPISODE UPLOAD] Upload successful.");

            return response.Content;
        }
        catch (ApiException ex)
        {
            Console.WriteLine(
                $"[EPISODE UPLOAD] API ERROR: " +
                $"{(int)ex.StatusCode} {ex.StatusCode}");

            throw new InvalidOperationException(
                $"آپلود ویدئو ناموفق بود. " +
                $"کد وضعیت: {(int)ex.StatusCode}",
                ex);
        }
    }


    // =========================================================
    // Get Episodes By Lesson
    // =========================================================

    public async Task<IReadOnlyList<CourseEpisodeDto>>
        GetByLessonIdAsync(Guid lessonId)
    {
        var accessToken =
            await _tokenStorage.GetAccessTokenAsync();

        Console.WriteLine(
            $"[EPISODES] AccessToken exists: " +
            $"{!string.IsNullOrWhiteSpace(accessToken)}");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "توکن ورود پیدا نشد. لطفاً دوباره وارد شوید.");
        }

        var authorization =
            $"Bearer {accessToken}";

        try
        {
            var response =
                await _api.GetByLessonIdAsync(
                    lessonId,
                    authorization);

            if (!response.IsSuccessStatusCode ||
                response.Content is null)
            {
                throw new InvalidOperationException(
                    $"دریافت ویدئوهای درس با خطا مواجه شد. " +
                    $"کد وضعیت: {(int)response.StatusCode}");
            }

            return response.Content;
        }
        catch (ApiException ex)
        {
            throw new InvalidOperationException(
                $"دریافت ویدئوهای درس ناموفق بود. " +
                $"کد وضعیت: {(int)ex.StatusCode}",
                ex);
        }
    }


    // =========================================================
    // Get Processing Status
    // =========================================================

    public async Task<CourseEpisodeDto>
        GetStatusAsync(
            Guid lessonId,
            Guid episodeId)
    {
        var accessToken =
            await _tokenStorage.GetAccessTokenAsync();

        Console.WriteLine(
            $"[EPISODE STATUS] AccessToken exists: " +
            $"{!string.IsNullOrWhiteSpace(accessToken)}");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "توکن ورود پیدا نشد. لطفاً دوباره وارد شوید.");
        }

        var authorization =
            $"Bearer {accessToken}";

        try
        {
            var response =
                await _api.GetStatusAsync(
                    lessonId,
                    episodeId,
                    authorization);

            if (!response.IsSuccessStatusCode ||
                response.Content is null)
            {
                throw new InvalidOperationException(
                    $"وضعیت پردازش ویدئو دریافت نشد. " +
                    $"کد وضعیت: {(int)response.StatusCode}");
            }

            return response.Content;
        }
        catch (ApiException ex)
        {
            throw new InvalidOperationException(
                $"دریافت وضعیت ویدئو ناموفق بود. " +
                $"کد وضعیت: {(int)ex.StatusCode}",
                ex);
        }
    }
}