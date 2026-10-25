using System.Globalization;
using System.Text;

namespace Marzi.Client.Shared.Utilities;

public static class PersianDateFormatter
{
    private static readonly PersianCalendar Calendar = new();
    private static readonly char[] PersianDigits = "۰۱۲۳۴۵۶۷۸۹".ToCharArray();

    public static string Format(DateTime? value, bool includeTime = true)
    {
        if (value is null || value.Value == default)
            return "—";

        var date = value.Value;
        var text = string.Create(CultureInfo.InvariantCulture, $"{Calendar.GetYear(date):0000}/{Calendar.GetMonth(date):00}/{Calendar.GetDayOfMonth(date):00}");
        if (includeTime)
            text += string.Create(CultureInfo.InvariantCulture, $" {date:HH:mm}");

        return ToPersianDigits(text);
    }

    public static string FormatMonthDay(DateTime? value)
    {
        if (value is null || value.Value == default)
            return "—";

        var date = value.Value;
        return ToPersianDigits(string.Create(CultureInfo.InvariantCulture,
            $"{Calendar.GetMonth(date):00}/{Calendar.GetDayOfMonth(date):00} {date:HH:mm}"));
    }

    public static string FormatYear(DateTime value) =>
        ToPersianDigits(Calendar.GetYear(value).ToString("0000", CultureInfo.InvariantCulture));

    private static string ToPersianDigits(string value)
    {
        var result = new StringBuilder(value.Length);
        foreach (var character in value)
            result.Append(character is >= '0' and <= '9' ? PersianDigits[character - '0'] : character);
        return result.ToString();
    }
}
