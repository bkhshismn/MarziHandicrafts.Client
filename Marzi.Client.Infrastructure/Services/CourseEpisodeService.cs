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
            $"[EPISODE UPLOAD] AccessToken exists: {!string.IsNullOrWhiteSpace(accessToken)}");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "توکن ورود پیدا نشد. لطفاً دوباره وارد شوید.");
        }
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidOperationException(
                "عنوان ویدئو را وارد کنید.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidOperationException(
                "توضیحات ویدئو را وارد کنید.");
        }

        if (order < 1)
        {
            throw new InvalidOperationException(
                "ترتیب ویدئو باید حداقل ۱ باشد.");
        }
        Console.WriteLine(
            $"[EPISODE UPLOAD] LessonId: {lessonId}");

        Console.WriteLine(
            $"[EPISODE UPLOAD] Title: {title}");

        Console.WriteLine(
            $"[EPISODE UPLOAD] Description length: {description?.Length ?? 0}");

        Console.WriteLine(
            $"[EPISODE UPLOAD] Order: {order}");

        Console.WriteLine(
            $"[EPISODE UPLOAD] IsFree: {isFree}");

        Console.WriteLine(
            $"[EPISODE UPLOAD] FileName: {fileName}");

        Console.WriteLine(
            $"[EPISODE UPLOAD] ContentType: {contentType}");

        if (stream.CanSeek)
        {
            Console.WriteLine(
                $"[EPISODE UPLOAD] Stream Length: {stream.Length}");
        }

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
                    title,
                    description,
                    order,
                    isFree,
                    filePart);

            Console.WriteLine(
                $"[EPISODE UPLOAD] Status: {(int)response.StatusCode} {response.StatusCode}");

            Console.WriteLine(
                $"[EPISODE UPLOAD] Has Content: {response.Content is not null}");

            if (response.Content is not null)
            {
                Console.WriteLine(
                    $"[EPISODE UPLOAD] Id: {response.Content.Id}");

                Console.WriteLine(
                    $"[EPISODE UPLOAD] Title: {response.Content.Title}");

                Console.WriteLine(
                    $"[EPISODE UPLOAD] VideoUrl: {response.Content.VideoUrl}");

                Console.WriteLine(
                    $"[EPISODE UPLOAD] Status: {response.Content.Status}");

                Console.WriteLine(
                    $"[EPISODE UPLOAD] DurationSeconds: {response.Content.DurationSeconds}");

                Console.WriteLine(
                    $"[EPISODE UPLOAD] Order: {response.Content.Order}");

                Console.WriteLine(
                    $"[EPISODE UPLOAD] IsFree: {response.Content.IsFree}");
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[EPISODE UPLOAD] Request failed.");

                if (response.Error is not null)
                {
                    Console.WriteLine(
                        $"[EPISODE UPLOAD] Error type: {response.Error.GetType().FullName}");

                    Console.WriteLine(
                        $"[EPISODE UPLOAD] Error: {response.Error}");

                    if (response.Error is ApiException apiException)
                    {
                        Console.WriteLine(
                            $"[EPISODE UPLOAD] Headers: {apiException.Headers}");
                    }
                }

                throw new InvalidOperationException(
                    $"آپلود ویدئو ناموفق بود. " +
                    $"کد وضعیت: {(int)response.StatusCode} " +
                    $"{response.StatusCode}");
            }

            if (response.Content is null)
            {
                throw new InvalidOperationException(
                    "سرور پاسخ موفق برگرداند ولی اطلاعات ویدئو خالی است.");
            }

            Console.WriteLine(
                "[EPISODE UPLOAD] Upload successful.");

            return response.Content;
        }
        catch (ApiException ex)
        {
            Console.WriteLine(
                $"[EPISODE UPLOAD] ApiException Status: {ex.StatusCode}");

            Console.WriteLine(
                $"[EPISODE UPLOAD] ApiException: {ex}");

            throw new InvalidOperationException(
                $"آپلود ویدئو ناموفق بود. " +
                $"کد وضعیت: {ex.StatusCode}",
                ex);
        }
    }

    public async Task<IReadOnlyList<CourseEpisodeDto>>
        GetByLessonIdAsync(Guid lessonId)
    {
        try
        {
            var response =
                await _api.GetByLessonIdAsync(lessonId);

            Console.WriteLine(
                $"[EPISODE LIST] Status: {(int)response.StatusCode} {response.StatusCode}");

            Console.WriteLine(
                $"[EPISODE LIST] Has Content: {response.Content is not null}");

            if (!response.IsSuccessStatusCode ||
                response.Content is null)
            {
                throw new InvalidOperationException(
                    $"دریافت ویدئوهای درس با خطا مواجه شد. " +
                    $"کد وضعیت: {(int)response.StatusCode}");
            }

            Console.WriteLine(
                $"[EPISODE LIST] Count: {response.Content.Count}");

            return response.Content;
        }
        catch (ApiException ex)
        {
            throw new InvalidOperationException(
                $"دریافت ویدئوهای درس ناموفق بود. " +
                $"کد وضعیت: {ex.StatusCode}",
                ex);
        }
    }

    public async Task<CourseEpisodeDto> GetStatusAsync(
    Guid lessonId,
    Guid episodeId)
    {
        try
        {
            Console.WriteLine(
                $"[EPISODE STATUS] GET /api/admin/lessons/{lessonId}/video/{episodeId}/status");

            var response =
                await _api.GetStatusAsync(
                    lessonId,
                    episodeId);

            Console.WriteLine(
                $"[EPISODE STATUS] HTTP: {(int)response.StatusCode} {response.StatusCode}");

            Console.WriteLine(
                $"[EPISODE STATUS] Content null: {response.Content is null}");

            Console.WriteLine(
                $"[EPISODE STATUS] Error null: {response.Error is null}");

            if (response.Content is not null)
            {
                Console.WriteLine(
                    $"[EPISODE STATUS] Id: {response.Content.Id}");

                Console.WriteLine(
                    $"[EPISODE STATUS] Title: {response.Content.Title}");

                Console.WriteLine(
                    $"[EPISODE STATUS] Status: {response.Content.Status}");

                Console.WriteLine(
                    $"[EPISODE STATUS] VideoUrl: {response.Content.VideoUrl}");

                Console.WriteLine(
                    $"[EPISODE STATUS] Duration: {response.Content.DurationSeconds}");

                return response.Content;
            }

            if (response.Error is not null)
            {
                Console.WriteLine(
                    $"[EPISODE STATUS] Error type: {response.Error.GetType().FullName}");

                Console.WriteLine(
                    $"[EPISODE STATUS] Error: {response.Error}");

                if (response.Error is ApiException apiException)
                {
                    Console.WriteLine(
                        $"[EPISODE STATUS] ApiException Status: {apiException.StatusCode}");

                    Console.WriteLine(
                        $"[EPISODE STATUS] ApiException Headers: {apiException.Headers}");
                }
            }

            throw new InvalidOperationException(
                $"سرور وضعیت ویدئو را با موفقیت برگرداند، " +
                $"اما اطلاعات قابل خواندن نبودند. " +
                $"HTTP: {(int)response.StatusCode} {response.StatusCode}");
        }
        catch (ApiException ex)
        {
            Console.WriteLine(
                $"[EPISODE STATUS] ApiException: {ex}");

            throw new InvalidOperationException(
                $"دریافت وضعیت ویدئو ناموفق بود. " +
                $"کد وضعیت: {(int)ex.StatusCode} {ex.StatusCode}",
                ex);
        }
    }

    public async Task DeleteAsync(Guid lessonId, Guid episodeId)
    {
        var response = await _api.DeleteAsync(lessonId, episodeId);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                "حذف ویدئو با خطا مواجه شد.");
        }
    }
}