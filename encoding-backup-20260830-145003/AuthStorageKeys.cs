namespace Marzi.Client.Web.Authentication;

public static class AuthStorageKeys
{
    public const string AccessToken = "auth.access_token";
    public const string RefreshToken = "auth.refresh_token";

    // اختیاری (اگر خواستی اطلاعات کاربر را هم نگه داری)
    public const string UserProfile = "auth.user_profile";
}
