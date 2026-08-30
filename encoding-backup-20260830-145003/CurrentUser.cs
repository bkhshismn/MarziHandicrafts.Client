namespace Marzi.Client.Core.Models;

public class CurrentUser
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string Mobile { get; set; } = "";
    public string Token { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public int ExpiresInMinutes { get; set; }
}