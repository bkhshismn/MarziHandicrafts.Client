using System.Security.Claims;
using System.Text.Json;

namespace Marzi.Client.Web.Authentication;

public static class JwtClaimsParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
            return [];

        var parts = jwt.Split('.');

        if (parts.Length != 3)
            return [];

        try
        {
            var jsonBytes = ParseBase64WithoutPadding(parts[1]);

            using var document = JsonDocument.Parse(jsonBytes);

            var claims = new List<Claim>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                var claimType = MapClaimType(property.Name);

                if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        var itemValue = item.ToString();

                        if (!string.IsNullOrWhiteSpace(itemValue))
                        {
                            claims.Add(
                                new Claim(claimType, itemValue));
                        }
                    }

                    continue;
                }

                var claimValue = property.Value.ToString();

                if (!string.IsNullOrWhiteSpace(claimValue))
                {
                    claims.Add(
                        new Claim(claimType, claimValue));
                }
            }

            return claims;
        }
        catch (JsonException)
        {
            return [];
        }
        catch (FormatException)
        {
            return [];
        }
    }

    private static string MapClaimType(string type)
    {
        return type switch
        {
            "sub" => ClaimTypes.NameIdentifier,
            "unique_name" => ClaimTypes.Name,
            "name" => ClaimTypes.Name,
            "role" => ClaimTypes.Role,
            "roles" => ClaimTypes.Role,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" => ClaimTypes.NameIdentifier,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" => ClaimTypes.Name,
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" => ClaimTypes.Role,
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid" => ClaimTypes.PrimarySid,
            _ => type
        };
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        base64 = base64
            .Replace('-', '+')
            .Replace('_', '/');

        return Convert.FromBase64String(
            base64.PadRight(
                base64.Length +
                (4 - base64.Length % 4) % 4,
                '='));
    }
}